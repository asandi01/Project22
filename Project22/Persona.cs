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
    class Persona {
        public int id {
            get; set;
        }

        public string identificacion {
            get; set;
        }
                       
        public string nombre {
            get; set;
        }
                         
        public string apellidos {
            get; set;
        }

        public string edad {
            get; set;
        }

        public string direccion {
            get; set;
        }
                        
        public string correo {
            get; set;
        }               

        public string telefono {
            get; set;
        }

        public DateTime fechaNac {
            get; set;
        }
                         
        public string sexo {
            get; set;
        }
                        
        public string estadoCivil {
            get; set;
        }
                        
        public string tipoSangre {
            get; set;
        }

        public string detalle {
            get; set;
        }

        public static explicit operator Persona(Java.Lang.Object v) {
            throw new NotImplementedException();
        }

    }
}