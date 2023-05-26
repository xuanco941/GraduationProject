
const jwt = require('jsonwebtoken');

const CheckAdmin = (req, res, next) => {
    if (!req.headers.authorization) res.status(401).json({ status: 'error', message: 'Request không có Access Token Admin' });
    else {
        const token = req.headers.authorization.split(' ')[1];
        jwt.verify(token, process.env.SECRET_KEY, (err, data) => {
            if (err) {
                res.status(403).json({ status: 'error', message: 'Check token failure' });
            }
            else{
                req.user = data;
                next();
            }
        })
    }

}

module.exports = CheckAdmin