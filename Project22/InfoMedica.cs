using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Project22 {
    class InfoMedica {

        public int id {
            get; set;
        }
        public int idPersona {
            get; set;
        }

        public string identificacion {
            get; set;
        }

        public double peso {
            get; set;
        }

        public double altura {
            get; set;
        }

        public double masaCorporal {
            get; set;
        }

        public string presionArterial {
            get; set;
        }

        public string frecuenciaCardiaca {
            get; set;
        }

        public string detalle {
            get; set;
        }

        public static explicit operator InfoMedica(Java.Lang.Object v) {
            throw new NotImplementedException();
        }
    }
}