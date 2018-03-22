using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace Efectos
{
    class Delay : ISampleProvider
    {
        private ISampleProvider fuente;

        private float atenuador;
        public float Atenuador
        {
            get
            {
                return atenuador;
            }

            set
            {
                if (value > 1)
                    atenuador = 1;
                else if (value < 0)
                    atenuador = 0;
                else
                    atenuador = value;
            }

        }

        int offsetTiempoMS;
        public int OffsetTiempoMS
                {
                    get
                    {
                        return offsetTiempoMS;
                    }

                    set
                    {
                        offsetTiempoMS = value;              
                    }

                }
        List<float> muestras = new List<float>();

        public Delay(ISampleProvider fuente)
        {
            this.fuente = fuente;
            offsetTiempoMS = 1000;
            //50ms - 5000ms

            Atenuador = 0.0f;
        }

        public Delay(ISampleProvider fuente, float atenuador)
        {
            this.fuente = fuente;
            Atenuador = atenuador;
            if (atenuador > 1)
                Atenuador = 1;
            else if (atenuador < 0)
                Atenuador = 0;
        }

        public WaveFormat WaveFormat
        {
            get
            {
                return fuente.WaveFormat;
            }
        }

        //Offset es el numero de muestras leidas
        public int Read(float[] buffer, int offset, int count)
        {
            //Calculo de tiempos
            var read = fuente.Read(buffer, offset, count);
            float tiempoTranscurrido =
               (float) muestras.Count / (float)fuente.WaveFormat.SampleRate;
            int muestrasTranscurridas = muestras.Count;
            float tiempoTranscurridoMS = tiempoTranscurrido * 1000;
            int numMuestraOffsetTiempo = (int)
                (((float)offsetTiempoMS / 1000.0f) * (float)fuente.WaveFormat.SampleRate);

            //Añadir muestrar al buffer
            for (int i = 0; i < read; i++)
            {
                muestras.Add(buffer[i]);
            }

            //Modificar muestras
            if (tiempoTranscurridoMS > offsetTiempoMS)
            {
                for (int i = 0; i < read; i++)
                {
                    buffer[i] += 
                        ((muestras[muestrasTranscurridas +
                        i - numMuestraOffsetTiempo]) *Atenuador);
                }

            }
            return read;
        }

        
    }
}
