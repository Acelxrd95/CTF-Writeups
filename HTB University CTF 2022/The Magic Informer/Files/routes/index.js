/** @format */

import path from "path";
import * as fs from "fs";
import axios from "axios";
import { Router } from "express";
import { sign } from "../helpers/JWTHelper.js";
import { AuthMiddleware } from "../middleware/AuthMiddleware.js";
import { AdminMiddleware } from "../middleware/AdminMiddleware.js";
import { LocalMiddleware } from "../middleware/LocalMiddleware.js";
import { execSync } from "child_process";

let db;
const router = Router();
const response = (data) => ({ message: data });

router.get("/", (req, res) => {
	return res.render("index.html");
});

router.get("/register", (req, res) => {
	return res.render("register.html");
});

router.get("/login", (req, res) => {
	return res.render("login.html");
});

router.post("/api/register", async (req, res) => {
	const { username, password } = req.body;

	if (username && password) {
		return db
			.getUser(username)
			.then((user) => {
				if (user)
					return res
						.status(401)
						.send(response("This username is already registered!"));
				return db
					.registerUser(username, password)
					.then(() =>
						res.send(response("Account registered successfully!"))
					);
			})
			.catch(() =>
				res.status(500).send(response("Something went wrong!"))
			);
	}
	return res
		.status(401)
		.send(response("Please fill out all the required fields!"));
});

router.post("/api/login", async (req, res) => {
	const { username, password } = req.body;

	if (username && password) {
		return db
			.loginUser(username, password)
			.then((user) => {
				let token = sign({ username: user.username });
				res.cookie("session", token, { maxAge: 3600000 });
				return res.send(response("User authenticated successfully!"));
			})
			.catch(() =>
				res.status(403).send(response("Invalid username or password!"))
			);
	}
	return res.status(500).send(response("Missing parameters!"));
});

router.get("/dashboard", AuthMiddleware, async (req, res) => {
	if (req.user.username === "admin") return res.redirect("/admin");

	return db
		.getUser(req.user.username)
		.then((user) => {
			if (!user) return res.redirect("/login");

			return db.getFormData(user.username).then((enrollment) => {
				res.render("dashboard.html", { user, enrollment });
			});
		})
		.catch((e) => {
			return res.redirect("/login");
		});
});

router.post("/api/enroll", AuthMiddleware, async (req, res) => {
	return db
		.getUser(req.user.username)
		.then((user) => {
			if (!user) return res.redirect("/login");

			const { full_name, phone, birth_date, gender, biography } =
				req.body;

			return db
				.updateEnrollment(
					full_name,
					phone,
					birth_date,
					gender,
					biography,
					user.username
				)
				.then(() =>
					res.send(
						response("Your information is saved successfully!")
					)
				)
				.catch((e) =>
					res.status(401).send(response("Something went wrong!"))
				);
		})
		.catch((e) => {
			return res.redirect("/login");
		});
});

router.post("/api/upload", AuthMiddleware, async (req, res) => {
	return db
		.getUser(req.user.username)
		.then(async (user) => {
			if (!user) return res.redirect("/login");

			if (!req.files || !req.files.resumeFile) {
				return res
					.status(400)
					.send(response("No files were uploaded."));
			}

			let enrollment = await db.getFormData(user.username);
			let resumeFile = req.files.resumeFile;
			let uploadFile = `${resumeFile.md5}.docx`;

			resumeFile.mv(path.join("/app/uploads", uploadFile), (err) => {
				if (err)
					return res
						.status(500)
						.send(response("Something went wrong!"));
			});

			if (
				enrollment.resume_file &&
				enrollment.resume_file !== uploadFile
			) {
				try {
					fs.unlinkSync(
						path.join("/app/uploads", enrollment.resume_file)
					);
				} catch (e) {
					console.log(e);
				}
			}

			return db
				.setResume(uploadFile, user.username)
				.then(() => {
					res.send({
						message: "Resume file uploaded successfully!",
						filename: uploadFile,
					});
				})
				.catch(() =>
					res.status(500).send(response("Something went wrong!"))
				);
		})
		.catch((e) => {
			return res.redirect("/login");
		});
});

router.get("/download", AuthMiddleware, async (req, res) => {
	return db
		.getUser(req.user.username)
		.then((user) => {
			if (!user) return res.redirect("/login");

			let { resume } = req.query;

			resume = resume.replaceAll("../", "");

			return res.download(path.join("/app/uploads", resume));
		})
		.catch((e) => {
			return res.redirect("/login");
		});
});

router.get("/admin", AdminMiddleware, async (req, res) => {
	return res.render("admin.html", { user: req.user });
});

router.get("/sms-settings", AdminMiddleware, async (req, res) => {
	return res.render("sms-settings.html", { user: req.user });
});

router.post("/api/sms/save", AdminMiddleware, async (req, res) => {
	const { verb, url, params, headers, resp_ok, resp_bad } = req.body;

	if (!(verb && url && params && headers && resp_ok && resp_bad)) {
		return res.status(500).send(response("missing required parameters"));
	}
	return db
		.saveSMSConfig(verb, url, params, headers, resp_ok, resp_bad)
		.then(() => {
			return res.send(response("SMS settings saved successfully!"));
		})
		.catch((e) => {
			return res.status(500).send(response("Something went wrong!"));
		});
});

router.post("/api/sms/test", AdminMiddleware, async (req, res) => {
	const { verb, url, params, headers, resp_ok, resp_bad } = req.body;

	if (!(verb && url && params && headers && resp_ok && resp_bad)) {
		return res.status(500).send(response("missing required parameters"));
	}

	let parsedHeaders = {};
	try {
		let headersArray = headers.split("\n");
		for (let header of headersArray) {
			if (header.includes(":")) {
				let hkey = header.split(":")[0].trim();
				let hval = header.split(":")[1].trim();
				parsedHeaders[hkey] = hval;
			}
		}
	} catch (e) {
		console.log(e);
	}

	let options = {
		method: verb.toLowerCase(),
		url: url,
		timeout: 5000,
		headers: parsedHeaders,
	};

	if (verb === "POST") options.data = params;

	axios(options)
		.then((response) => {
			if (typeof response.data == "object") {
				response.data = JSON.stringify(response.data);
			}
			return res.json({ status: "success", result: response.data });
		})
		.catch((e) => {
			if (e.response) {
				if (typeof e.response.data == "object") {
					e.response.data = JSON.stringify(e.response.data);
				}
				return res.json({ status: "fail", result: e.response.data });
			} else {
				return res.json({
					status: "fail",
					result: "Address is unreachable",
				});
			}
		});
});

router.get("/sql-prompt", AdminMiddleware, async (req, res) => {
	return res.render("sql-prompt.html", { user: req.user });
});

router.post(
	"/debug/sql/exec",
	LocalMiddleware,
	AdminMiddleware,
	async (req, res) => {
		const { sql, password } = req.body;

		if (sql && password === process.env.DEBUG_PASS) {
			try {
				let safeSql = String(sql).replaceAll(/"/gi, "'");

				let cmdStr = `sqlite3 -csv admin.db "${safeSql}"`;

				const cmdExec = execSync(cmdStr);

				return res.json({ sql, output: cmdExec.toString() });
			} catch (e) {
				let output = e.toString();
				if (e.stderr) output = e.stderr.toString();
				return res.json({ sql, output });
			}
		}

		return res
			.status(500)
			.send(response("Invalid debug password supplied!"));
	}
);

router.get("/logout", (req, res) => {
	res.clearCookie("session");
	return res.redirect("/");
});

export default (database) => {
	db = database;
	return router;
};
