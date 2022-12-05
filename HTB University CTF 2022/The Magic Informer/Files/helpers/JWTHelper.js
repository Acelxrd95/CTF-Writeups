/** @format */

import jwt from "jsonwebtoken";
import crypto from "crypto";
const APP_SECRET = crypto.randomBytes(69).toString("hex");

const sign = (data) => {
	data = Object.assign(data);
	return jwt.sign(data, APP_SECRET, { algorithm: "HS256" });
};

const decode = async (token) => {
	return jwt.decode(token);
};

export { sign, decode };
