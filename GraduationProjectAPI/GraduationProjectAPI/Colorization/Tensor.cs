//*************************************************************************************************
//* (C) ColorfulSoft corp., 2021 - 2022. All Rights reserved.
//*************************************************************************************************

using System;
using System.Runtime.InteropServices;

namespace GraduationProjectAPI.Colorization
{

    /// <summary>
    /// Multidimentional array of floating point data type.
    /// </summary>
    public sealed unsafe class Tensor : IDisposable
    {

        /// <summary>
        /// Data.
        /// </summary>
        public float* Data;

        /// <summary>
        /// Should destructor free Data?
        /// </summary>
        private bool __DisposeData = true;

        /// <summary>
        /// Shape.
        /// </summary>
        public int* Shape;

        /// <summary>
        /// Number of elements.
        /// </summary>
        public int Numel;

        /// <summary>
        /// Number of dimentions.
        /// </summary>
        public int Ndim;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Tensor()
        {
        }

        /// <summary>
        /// Initializes the tensor with specified shape.
        /// </summary>
        /// <param name="shape">Shape.</param>
        public Tensor(params int[] shape)
        {
            Ndim = shape.Length;
            Numel = 1;
            Shape = (int*)Marshal.AllocCoTaskMem(sizeof(int) * Ndim).ToPointer();
            var Pshape = Shape;
            foreach (var Dim in shape)
            {
                Numel *= Dim;
                *Pshape++ = Dim;
            }
            Data = (float*)Marshal.AllocCoTaskMem(sizeof(float) * Numel).ToPointer();
        }

        /// <summary>
        /// Disposes unmanaged resources of the tensor.
        /// </summary>
        void IDisposable.Dispose()
        {
            if (Data != null && __DisposeData)
            {
                Marshal.FreeCoTaskMem((IntPtr)Data);
                Data = null;
            }
            if (Shape != null)
            {
                Marshal.FreeCoTaskMem((IntPtr)Shape);
                Shape = null;
            }
        }

        /// <summary>
        /// Disposes unmanaged resources of the tensor.
        /// </summary>
        ~Tensor()
        {
            if (Data != null && __DisposeData)
            {
                Marshal.FreeCoTaskMem((IntPtr)Data);
                Data = null;
            }
            if (Shape != null)
            {
                Marshal.FreeCoTaskMem((IntPtr)Shape);
                Shape = null;
            }
        }

        /// <summary>
        /// Flattens 3d tensor to 2d.
        /// </summary>
        /// <returns>Tensor.</returns>
        public Tensor Flat3d()
        {
            var t = new Tensor();
            t.Data = Data;
            t.Ndim = 2;
            t.Numel = Numel;
            t.Shape = (int*)Marshal.AllocCoTaskMem(sizeof(int) * 2).ToPointer();
            t.Shape[0] = Shape[0];
            t.Shape[1] = Shape[1] * Shape[2];
            __DisposeData = false;
            return t;
        }

        /// <summary>
        /// Unflats the 2d tensor to 3d using specified size.
        /// </summary>
        /// <param name="h">Height.</param>
        /// <param name="w">Width.</param>
        /// <returns>Tensor.</returns>
        public Tensor Unflat3d(int h, int w)
        {
            var t = new Tensor();
            t.Data = Data;
            t.Ndim = 3;
            t.Numel = Numel;
            t.Shape = (int*)Marshal.AllocCoTaskMem(sizeof(int) * 3).ToPointer();
            t.Shape[0] = Shape[0];
            t.Shape[1] = h;
            t.Shape[2] = w;
            __DisposeData = false;
            return t;
        }

        /// <summary>
        /// Returns transposed version of this tensor.
        /// </summary>
        /// <returns>Tensor.</returns>
        public Tensor Transpose2d()
        {
            var t = new Tensor(Shape[1], Shape[0]);
            var px = Data;
            var py = t.Data;
            var width = Shape[1];
            var height = Shape[0];
            var n = 0;
            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    py[j * height + i] = px[i * width + j];
                    ++n;
                }
            }
            return t;
        }

    }

}
