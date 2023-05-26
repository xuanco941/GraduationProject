const fs = require('fs');


class ImageCaptioningPipeline {
  static task = 'image-to-text';
  static model = 'Xenova/vit-gpt2-image-captioning';
  static instance = null;

  static async getInstance(progress_callback = null) {
    if (this.instance === null) {
      // Dynamically import the Transformers.js library
      let { pipeline, env } = await import('@xenova/transformers');
      env.cacheDir = './.cache';
      this.instance = pipeline(this.task, this.model, { progress_callback });
    }
    return this.instance;
  }

}




const ImageCaptioning = async (req, res) => {
  let file = req.file;
  if (file) {
    let imageToText = await ImageCaptioningPipeline.getInstance();
    let { RawImage } = await import('@xenova/transformers');
    const sizeOf = require('image-size');
    const dimensions = sizeOf(file.buffer);
    const width = dimensions.width;
    const height = dimensions.height;
    let rawImage;
    if (file.mimetype === 'image/png') {
      rawImage = new RawImage(file.buffer, width, height, 4); 
    } else {
      rawImage = new RawImage(file.buffer, width, height, 3);
    } 
    let output = await imageToText(rawImage);
    return res.status(200).json(output);
  } else {
    return res.status(400);
  }
}

module.exports = ImageCaptioning;
