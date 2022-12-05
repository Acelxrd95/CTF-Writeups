/** @format */

import * as dotenv from "dotenv";
import cookieParser from "cookie-parser";
import path from "path";
import express from "express";
import nunjucks from "nunjucks";
import fileUpload from "express-fileupload";
import * as router from "./routes/index.js";
import { Database } from "./database.js";

dotenv.config({ path: "/app/debug.env" });

const app = express();
const db = new Database("admin.db");

app.use(express.json());
app.use(cookieParser());
app.use(
	fileUpload({
		limits: {
			fileSize: 2 * 1024 * 1024, // 2 MB
		},
		abortOnLimit: true,
	})
);

nunjucks.configure("views", {
	autoescape: true,
	express: app,
});

app.disable("etag");

app.set("views", "./views");
app.use("/static", express.static(path.resolve("static")));

app.use(router.default(db));

app.all("*", (req, res) => {
	return res.status(404).send({
		message: "404 page not found",
	});
});

(async () => {
	await db.connect();
	await db.migrate();
	app.listen(1337, "0.0.0.0", () => console.log("Listening on port 1337"));
})();
