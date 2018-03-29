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
    [Activity(Label = "Informacion Medica")]
    public class InfoMedicaActivity : Activity {
        string personaId, personaNombre, personaIdentificacion;
        Button btnAdd;  
        ListView lv;
        IList<InfoMedica> listItsms = null;
        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.InfoMedica);

            //Optener el dato de la persona
            personaId= Intent.GetStringExtra("PersonaId")??string.Empty;
            personaNombre=Intent.GetStringExtra("PersonaNombre")??string.Empty;
            personaIdentificacion=Intent.GetStringExtra("PersonaIdentificacion")??string.Empty;

            btnAdd=FindViewById<Button>(Resource.Id.infoMedicaListBtnAdd);        
            lv=FindViewById<ListView>(Resource.Id.infoMedicaListListView);

            btnAdd.Click+=delegate {
                var activityAddEdit = new Intent(this, typeof(AddEditPersonaActivity));
                activityAddEdit.PutExtra("PersonaId", personaId);
                activityAddEdit.PutExtra("PersonaNombre", personaNombre);
                activityAddEdit.PutExtra("PersonaIdentificacion", personaIdentificacion);
                StartActivity(activityAddEdit);
            };

            LoadInfoMedicaInList();

        }

        private void LoadInfoMedicaInList() {
            PersonaDbHelper dbVals = new PersonaDbHelper(this);  
            listItsms=dbVals.GetAllPersonas();

            lv.Adapter=new PersonaListBaseAdapter(this, listItsms);

            lv.ItemLongClick+=lv_ItemLongClick;
        }

        private void lv_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e) {
            Persona o = listItsms[e.Position];

            var activityAddEdit = new Intent(this, typeof(AddEditPersonaActivity));
            activityAddEdit.PutExtra("PersonaId", o.id.ToString());
            activityAddEdit.PutExtra("PersonaName", o.nombre);
            StartActivity(activityAddEdit);
        }
    }
}