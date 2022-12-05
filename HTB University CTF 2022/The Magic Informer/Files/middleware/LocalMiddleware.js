const LocalMiddleware = async (req, res, next) => {
    if (req.ip == '127.0.0.1' && req.headers.host == '127.0.0.1:1337') {
        return next();
    }
    return res.status(401).json({ message: 'Blocked: This endpoint is whitelisted to localhost only.' });
}

export { LocalMiddleware };