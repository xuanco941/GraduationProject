
class TranslationPipeline {
  static task = 'translation';
  static model = 'Xenova/nllb-200-distilled-600M';
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

const Translation = async (req, res) => {
  const text = req.body.text;
  const tgt_lang = req.body.tgt_lang;
  const src_lang = req.body.src_lang;

  if (text) {
    try {
      let translator = await TranslationPipeline.getInstance();

      let output = await translator(text, {
        tgt_lang: tgt_lang,
        src_lang: src_lang
      });
      return res.status(200).json(output);
    }
    catch {
      return res.status(500);
    }

  }
  else {
    return res.status(400);
  }
}

module.exports = Translation;
