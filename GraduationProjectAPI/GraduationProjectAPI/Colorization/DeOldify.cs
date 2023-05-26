using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProjectAPI.Colorization
{
    public unsafe class DeOldify
    {
        public Dictionary<string, Tensor> Parameters { get; set; }
        private Functional functional { get; set; }

        public DeOldify()
        {
            Parameters = new Dictionary<string, Tensor>(DeOldifyModel.Parameters);
            functional = new Functional();
        }




        ///<summary>
        /// Converts Bitmap to Tensor.
        ///</summary>
        ///<param name="bmp">Source.</param>
        ///<returns>Tensor with pixels.</returns>
        public Tensor Image2Tensor(Bitmap bmp)
        {
            var t = new Tensor(3, bmp.Height, bmp.Width);
            var pt = t.Data;
            for (int y = 0; y < bmp.Height; ++y)
            {
                for (int x = 0; x < bmp.Width; ++x)
                {
                    var c = bmp.GetPixel(x, y);
                    var l = (c.R + c.G + c.B) / 765f;
                    pt[y * bmp.Width + x] = (l - 0.485f) / 0.229f;
                    pt[(bmp.Height + y) * bmp.Width + x] = (l - 0.456f) / 0.224f;
                    pt[(2 * bmp.Height + y) * bmp.Width + x] = (l - 0.406f) / 0.225f;
                }
            }
            return t;
        }

        ///<summary>
        /// Converts Tensor to Bitmap.
        ///</summary>
        ///<param name="t">Source.</param>
        ///<returns>Bitmap with pixels from Tensor t.</returns>
        public Bitmap Tensor2Image(Tensor t)
        {
            var bmp = new Bitmap(t.Shape[2], t.Shape[1]);
            for (int y = 0; y < t.Shape[1]; ++y)
            {
                for (int x = 0; x < t.Shape[2]; ++x)
                {
                    bmp.SetPixel(x, y, Color.FromArgb((byte)Math.Min(Math.Max(((t.Data[y * t.Shape[2] + x] * 6f - 3f) * 0.229f + 0.485f) * 255f, 0f), 255f),
                                                      (byte)Math.Min(Math.Max(((t.Data[(t.Shape[1] + y) * t.Shape[2] + x] * 6f - 3f) * 0.224f + 0.456f) * 255f, 0f), 255f),
                                                      (byte)Math.Min(Math.Max(((t.Data[(2 * t.Shape[1] + y) * t.Shape[2] + x] * 6f - 3f) * 0.225f + 0.406f) * 255f, 0f), 255f)));
                }
            }
            return bmp;
        }

        ///<summary>
        /// Transfers colors from colorized image to original B&W image.
        ///</summary>
        ///<param name="full_size">Original B&W image.</param>
        ///<param name="colorized">Colorized image.</param>
        ///<returns>HR bitmap with content from full_size and colors from colorized.</returns>
        public Bitmap Mux(Bitmap full_size, Bitmap colorized)
        {
            var colorized_ = new Bitmap(colorized, full_size.Width, full_size.Height);
            for (int y = 0; y < colorized_.Height; ++y)
            {
                for (int x = 0; x < colorized_.Width; ++x)
                {
                    var bwc = full_size.GetPixel(x, y);
                    var rc = colorized_.GetPixel(x, y);
                    var bwy = 0.299f * bwc.R + 0.587f * bwc.G + 0.114f * bwc.B;
                    var ru = -0.14713f * rc.R - 0.28886f * rc.G + 0.436f * rc.B;
                    var rv = 0.615f * rc.R - 0.51499f * rc.G - 0.10001f * rc.B;
                    colorized_.SetPixel(x, y, Color.FromArgb((byte)Math.Min(Math.Max(bwy + 1.139837398373983740f * rv, 0f), 255f),
                                                             (byte)Math.Min(Math.Max(bwy - 0.3946517043589703515f * ru - 0.5805986066674976801f * rv, 0f), 255f),
                                                             (byte)Math.Min(Math.Max(bwy + 2.032110091743119266f * ru, 0f), 255f)));
                }
            }
            return colorized_;
        }

        ///<summary>
        /// Changes the image size to 256 on the smaller side.
        ///</summary>
        ///<param name="bmp">Source.</param>
        ///<returns>Resized bitmap.</returns>
        public Bitmap Resize(Bitmap bmp)
        {
            if (bmp.Width > bmp.Height)
            {
                return new Bitmap(bmp, (int)(256f / bmp.Height * bmp.Width), 256);
            }
            else
            {
                return new Bitmap(bmp, 256, (int)(256f / bmp.Width * bmp.Height));
            }
        }

        ///<summary>
        /// Executes Functional.Conv2d with parameters from ckpt.
        ///</summary>
        ///<param name="x">Source.</param>
        ///<param name="layer">Layer name.</param>
        ///<param name="ckpt">State dict.</param>
        ///<param name="stride">Stride.</param>
        ///<param name="padding">Padding.</param>
        ///<returns>Tensor.</returns>
        public Tensor Conv2d(Tensor x, string layer, Dictionary<string, Tensor> ckpt, int stride = 1, int padding = 1)
        {
            var y = functional.Conv2d(x, ckpt[layer + ".weight"], ckpt.ContainsKey(layer + ".bias") ? ckpt[layer + ".bias"] : null, padding, padding, padding, padding, stride, stride, 1, 1, 1);
            return y;
        }

        ///<summary>
        /// Executes Functional.BatchNorm2d_ with parameters from ckpt.
        ///</summary>
        ///<param name="x">Source.</param>
        ///<param name="layer">Layer name.</param>
        ///<param name="ckpt">State dict.</param>
        ///<returns>Tensor.</returns>
        public Tensor BatchNorm2d(Tensor x, string layer, Dictionary<string, Tensor> ckpt)
        {
            return functional.BatchNorm2d_(x, ckpt[layer + ".running_mean"], ckpt[layer + ".running_var"], ckpt[layer + ".weight"], ckpt[layer + ".bias"]);
        }

        ///<summary>
        /// Executes BasicBlock of DeOldify backbone with parameters from ckpt.
        ///</summary>
        ///<param name="x">Source.</param>
        ///<param name="layer">Layer name.</param>
        ///<param name="ckpt">State dict.</param>
        ///<param name="strided">Is stride == 2?</param>
        ///<returns>Tensor.</returns>
        public Tensor BasicBlock(Tensor x, string layer, Dictionary<string, Tensor> ckpt, bool strided = false)
        {
            var @out = Conv2d(x, layer + ".conv1", ckpt, 1, 0);
            @out = BatchNorm2d(@out, layer + ".bn1", ckpt);
            @out = functional.ReLU_(@out);
            @out = Conv2d(@out, layer + ".conv2", ckpt, strided ? 2 : 1, 1);
            @out = BatchNorm2d(@out, layer + ".bn2", ckpt);
            @out = functional.ReLU_(@out);
            @out = Conv2d(@out, layer + ".conv3", ckpt, 1, 0);
            @out = BatchNorm2d(@out, layer + ".bn3", ckpt);
            if (@out.Shape[0] != x.Shape[0] || strided)
            {
                x = BatchNorm2d(Conv2d(x, layer + ".downsample.0", ckpt, strided ? 2 : 1, 0), layer + ".downsample.1", ckpt);
            }
            return functional.ReLU_(functional.Plus_(@out, x));

        }

        ///<summary>
        /// Executes Middle of DeOldify with parameters from ckpt.
        ///</summary>
        ///<param name="x">Source.</param>
        ///<param name="layer">Layer name.</param>
        ///<param name="ckpt">State dict.</param>
        ///<returns>Tensor.</returns>
        public Tensor MiddleBlock(Tensor x, string layer, Dictionary<string, Tensor> ckpt)
        {
            return BatchNorm2d(functional.ReLU_(Conv2d(BatchNorm2d(functional.ReLU_(Conv2d(x, layer + ".0.0", ckpt)), layer + ".0.2", ckpt), layer + ".1.0", ckpt)), layer + ".1.2", ckpt);
        }

        ///<summary>
        /// Executes CustomPixelShuffle of DeOldify with parameters from ckpt.
        ///</summary>
        ///<param name="x">Source.</param>
        ///<param name="layer">Layer name.</param>
        ///<param name="ckpt">State dict.</param>
        ///<returns>Tensor.</returns>
        public Tensor CustomPixelShuffle(Tensor x, string layer, Dictionary<string, Tensor> ckpt)
        {
            return functional.AvgPool2d(functional.PixelShuffle(functional.ReLU_(BatchNorm2d(Conv2d(x, layer + ".conv.0", ckpt, padding: 0), layer + ".conv.1", ckpt))), 2, 2, 1, 1, 1, 1, 1, 1);
        }

        ///<summary>
        /// Executes UnetBlockWide of DeOldify with parameters from ckpt.
        ///</summary>
        ///<param name="up_in">Source.</param>
        ///<param name="s">Source.</param>
        ///<param name="layer">Layer name.</param>
        ///<param name="ckpt">State dict.</param>
        ///<returns>Tensor.</returns>
        public Tensor UnetBlockWide(Tensor up_in, Tensor s, string layer, Dictionary<string, Tensor> ckpt, bool self_attentional = false)
        {
            var up_out = CustomPixelShuffle(up_in, layer + ".shuf", ckpt);
            var cat_x = functional.ReLU_(functional.RestrictedCat2d(up_out, BatchNorm2d(s, layer + ".bn", ckpt)));
            if (self_attentional)
            {
                return SelfAttention(BatchNorm2d(functional.ReLU_(Conv2d(cat_x, layer + ".conv.0", ckpt)), layer + ".conv.2", ckpt), layer + ".conv.3", ckpt);
            }
            return BatchNorm2d(functional.ReLU_(Conv2d(cat_x, layer + ".conv.0", ckpt)), layer + ".conv.2", ckpt);
        }


        ///<summary>
        /// Executes SelfAttention of DeOldify with parameters from ckpt.
        ///</summary>
        ///<param name="x">Source.</param>
        ///<param name="layer">Layer name.</param>
        ///<param name="ckpt">State dict.</param>
        ///<returns>Tensor.</returns>
        public Tensor SelfAttention(Tensor x, string layer, Dictionary<string, Tensor> ckpt)
        {
            var gamma = ckpt[layer + ".gamma"].Data[0];
            var f = Conv2d(x, layer + ".query", ckpt, padding: 0).Flat3d();
            var g = Conv2d(x, layer + ".key", ckpt, padding: 0).Flat3d();
            var h = Conv2d(x, layer + ".value", ckpt, padding: 0).Flat3d();
            var beta = functional.Softmax2d(functional.MatMul(f.Transpose2d(), g));
            return functional.Plus_(functional.EltwiseMulScalar_(functional.MatMul(h, beta), gamma).Unflat3d(x.Shape[1], x.Shape[2]), x);
        }

        ///<summary>
        /// Executes PixelShuffle of DeOldify with parameters from ckpt.
        ///</summary>
        ///<param name="x">Source.</param>
        ///<param name="layer">Layer name.</param>
        ///<param name="ckpt">State dict.</param>
        ///<returns>Tensor.</returns>
        public Tensor PixelShuffle(Tensor x, string layer, Dictionary<string, Tensor> ckpt)
        {
            return functional.AvgPool2d(functional.ReLU_(functional.PixelShuffle(Conv2d(x, layer + ".conv.0", ckpt, padding: 0))), 2, 2, 1, 1, 1, 1, 1, 1);
        }

        ///<summary>
        /// Executes ResBlock of DeOldify with parameters from ckpt.
        ///</summary>
        ///<param name="x">Source.</param>
        ///<param name="layer">Layer name.</param>
        ///<param name="ckpt">State dict.</param>
        ///<returns>Tensor.</returns>
        public Tensor ResBlock(Tensor x, string layer, Dictionary<string, Tensor> ckpt)
        {
            var @out = Conv2d(x, layer + ".layers.0.0", ckpt);
            @out = functional.ReLU_(@out);
            @out = Conv2d(@out, layer + ".layers.1.0", ckpt);
            @out = functional.ReLU_(@out);
            return functional.Plus_(@out, x);
        }

        public Bitmap Colorize(Bitmap bw)
        {
            var x = Image2Tensor(Resize(bw));
            // Net
            var x1 = functional.ReLU_(
                BatchNorm2d(
                    Conv2d(x, "layers.0.0", Parameters, 2, 3), "layers.0.1", Parameters));
            var x2 = BasicBlock(
                BasicBlock(
                    BasicBlock(
                        functional.MaxPool2d(x1, 3, 3, 2, 2, 1, 1, 1, 1), "layers.0.4.0", Parameters), "layers.0.4.1", Parameters), "layers.0.4.2", Parameters);
            var x3 = BasicBlock(
                BasicBlock(
                    BasicBlock(
                        BasicBlock(x2, "layers.0.5.0", Parameters, true), "layers.0.5.1", Parameters), "layers.0.5.2", Parameters), "layers.0.5.3", Parameters);
            var x4 = BasicBlock(
                BasicBlock(
                    BasicBlock(
                        BasicBlock(
                            BasicBlock(x3, "layers.0.6.0", Parameters, true), "layers.0.6.1", Parameters), "layers.0.6.2", Parameters), "layers.0.6.3", Parameters), "layers.0.6.4", Parameters);
            x4 = BasicBlock(
                BasicBlock(
                    BasicBlock(
                        BasicBlock(
                            BasicBlock(x4, "layers.0.6.5", Parameters), "layers.0.6.6", Parameters), "layers.0.6.7", Parameters), "layers.0.6.8", Parameters), "layers.0.6.9", Parameters);
            x4 = BasicBlock(
                BasicBlock(
                    BasicBlock(
                        BasicBlock(
                            BasicBlock(x4, "layers.0.6.10", Parameters), "layers.0.6.11", Parameters), "layers.0.6.12", Parameters), "layers.0.6.13", Parameters), "layers.0.6.14", Parameters);
            x4 = BasicBlock(
                BasicBlock(
                    BasicBlock(
                        BasicBlock(
                            BasicBlock(x4, "layers.0.6.15", Parameters), "layers.0.6.16", Parameters), "layers.0.6.17", Parameters), "layers.0.6.18", Parameters), "layers.0.6.19", Parameters);
            x4 = BasicBlock(
                BasicBlock(
                    BasicBlock(x4, "layers.0.6.20", Parameters), "layers.0.6.21", Parameters), "layers.0.6.22", Parameters);
            var x5 = BasicBlock(
                BasicBlock(
                    BasicBlock(x4, "layers.0.7.0", Parameters, true), "layers.0.7.1", Parameters), "layers.0.7.2", Parameters);
            var y = functional.ReLU_(BatchNorm2d(x5, "layers.1", Parameters));
            y = MiddleBlock(y, "layers.3", Parameters);
            y = UnetBlockWide(y, x4, "layers.4", Parameters);
            y = UnetBlockWide(y, x3, "layers.5", Parameters, true);
            y = UnetBlockWide(y, x2, "layers.6", Parameters);
            y = UnetBlockWide(y, x1, "layers.7", Parameters);
            y = PixelShuffle(y, "layers.8", Parameters);
            y = functional.RestrictedCat2d(y, x);
            y = ResBlock(y, "layers.10", Parameters);
            y = Conv2d(y, "layers.11.0", Parameters, padding: 1);
            y = functional.Sigmoid_(y);

            return Mux(bw, Tensor2Image(y));
        }
    }
}
