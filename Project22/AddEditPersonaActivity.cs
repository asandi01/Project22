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
using Android.Database;
using System.Text.RegularExpressions;
using Java.Interop;

namespace Project22 {
    [Activity(Label = "Add/Edit Persona")]
    public class AddEditPersonaActivity : Activity {
        EditText etid, etidentificacion, etnombre, etapellidos, etdireccion, etcorreo, ettelefono, etestadocivil, ettiposangre, etdetalle, etsexo;
        EditText dpfechanac;
        Button btninsertar;

        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddEditPersona);  

            etid=FindViewById<EditText>(Resource.Id.etid);
            etidentificacion=FindViewById<EditText>(Resource.Id.etidentificacion);
            etnombre=FindViewById<EditText>(Resource.Id.etnombre);
            etapellidos=FindViewById<EditText>(Resource.Id.etapellidos);
            etdireccion=FindViewById<EditText>(Resource.Id.etdireccion);
            etcorreo=FindViewById<EditText>(Resource.Id.etcorreo);
            ettelefono=FindViewById<EditText>(Resource.Id.ettelefono);
            etsexo=FindViewById<EditText>(Resource.Id.etsexo);
            etestadocivil=FindViewById<EditText>(Resource.Id.etestadocivil);
            etdetalle=FindViewById<EditText>(Resource.Id.etdetalle);
            ettiposangre=FindViewById<EditText>(Resource.Id.ettiposangre);
            dpfechanac=FindViewById<EditText>(Resource.Id.dpfechanac);
            btninsertar=FindViewById<Button>(Resource.Id.btninsertar);

            btninsertar.Click+=buttonInsertClick;

            string editId = Intent.GetStringExtra("PersonaId")??string.Empty;

            if (editId.Trim().Length>0) {
                etid.Text=editId;
                LoadDataForEdit(editId);
            }
        }

        private void LoadDataForEdit(string contactId) {
            PersonaDbHelper db = new PersonaDbHelper(this);
            ICursor cData = db.getPersonaById(int.Parse(contactId));
            if (cData.MoveToFirst()) {                                                  
                etidentificacion.Text=cData.GetString(cData.GetColumnIndex("identificacion"));
                etnombre.Text=cData.GetString(cData.GetColumnIndex("nombre"));
                etapellidos.Text=cData.GetString(cData.GetColumnIndex("apellidos"));
                etdireccion.Text=cData.GetString(cData.GetColumnIndex("direccion"));

                etcorreo.Text=cData.GetString(cData.GetColumnIndex("correo"));
                ettelefono.Text=cData.GetString(cData.GetColumnIndex("telefono"));
                etsexo.Text=cData.GetString(cData.GetColumnIndex("sexo"));

                etestadocivil.Text=cData.GetString(cData.GetColumnIndex("estadoCivil"));
                etdetalle.Text=cData.GetString(cData.GetColumnIndex("detalle"));
                ettiposangre.Text=cData.GetString(cData.GetColumnIndex("tipoSangre"));

                dpfechanac.Text=cData.GetString(cData.GetColumnIndex("fechaNac"));
            }
        }

        void buttonInsertClick(object sender, EventArgs e) {
            PersonaDbHelper db = new PersonaDbHelper(this);
            if (etidentificacion.Text.Trim().Length<1) {
                Toast.MakeText(this, "Ingrese la identificacion.", ToastLength.Short).Show();
                return;
            }
            if (etnombre.Text.Trim().Length<1) {
                Toast.MakeText(this, "Ingrese el nombre.", ToastLength.Short).Show();
                return;
            }

            if (etapellidos.Text.Trim().Length<1) {
                Toast.MakeText(this, "Ingrese los apellidos.", ToastLength.Short).Show();
                return;
            }

            if (etdireccion.Text.Trim().Length<1) {
                Toast.MakeText(this, "Ingrese la direccion.", ToastLength.Short).Show();
                return;
            }

            if (dpfechanac.Text.Trim().Length<1) {
                string fechaPattern = "^(0[1-9]|[12][0-9]|3[01])[.](0[1-9]|1[012])[.](19|20)[0-9]{2}$";
                                        
                if (!Regex.IsMatch(dpfechanac.Text, fechaPattern, RegexOptions.IgnoreCase)) {
                    Toast.MakeText(this, "Fecha de nacimiento incorrecta.", ToastLength.Short).Show();
                    return;
                }
                Toast.MakeText(this, "Ingrese la fecha de nacimiento.", ToastLength.Short).Show();
                return;
            }

            if (etcorreo.Text.Trim().Length>0) {
                string EmailPattern = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                if (!Regex.IsMatch(etcorreo.Text, EmailPattern, RegexOptions.IgnoreCase)) {
                    Toast.MakeText(this, "Correo incorrecto.", ToastLength.Short).Show();
                    return;
                }
            }

            Persona per = new Persona();

            if (etid.Text.Trim().Length>0) {
                per.id=int.Parse(etid.Text);
            }
            per.identificacion=etidentificacion.Text;
            per.nombre=etnombre.Text;
            per.apellidos=etapellidos.Text;
            per.direccion=etdireccion.Text;
            per.correo=etcorreo.Text;

            per.telefono=ettelefono.Text;
            per.estadoCivil=etestadocivil.Text;
            per.tipoSangre=ettiposangre.Text;
            per.detalle=etdetalle.Text;
            per.sexo=etsexo.Text;
            try {
                per.fechaNac=Convert.ToDateTime(dpfechanac.Text);
            } catch {
                per.fechaNac=new DateTime();
            }                                                 

            try {

                if (etid.Text.Trim().Length>0) {
                    db.UpdatePersona(per);
                    Toast.MakeText(this, "Se actualizo correctamente.", ToastLength.Short).Show();
                } else {
                    db.AddNewPersona(per);
                    Toast.MakeText(this, "Se agrego correctamente.", ToastLength.Short).Show();
                }

                Finish();

                //Go to main activity after save/edit
                var mainActivity = new Intent(this, typeof(PersonaActivity));
                StartActivity(mainActivity);

            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }

        }
    }
}