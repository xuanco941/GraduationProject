const express = require('express');
const router = express.Router();
const Translation = require('../controller/Translation');
const ImageCaptioning = require('../controller/ImageCaptioning');

const multer = require('multer');

//disk
// const storage = multer.diskStorage({
//     destination: function (req, file, cb) {
//         cb(null, 'uploads/')
//     },
//     filename: function (req, file, cb) {
//         cb(null, file.originalname)
//     }
// });
const storage = multer.memoryStorage();
const upload = multer({
    storage: storage,
    fileFilter: (req, file, cb) => {
        if (file.mimetype.startsWith('image/')) {
            cb(null, true);
        } else {
            cb(new Error('Only image files are allowed.'));
        }
    },
});
router.post("/image-captioning", upload.single('image'), ImageCaptioning);

router.post("/translation", Translation);

module.exports = router;