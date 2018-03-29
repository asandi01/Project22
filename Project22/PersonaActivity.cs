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
    [Activity(Label = "Personas")]
    public class PersonaActivity : Activity {
        Button btnAdd, btnSearch;
        EditText txtSearch;
        ListView lv;
        IList<Persona> listItsms = null;
        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Personas);

            btnAdd=FindViewById<Button>(Resource.Id.personaListBtnAdd);
            btnSearch=FindViewById<Button>(Resource.Id.personaListBtnSearch);
            txtSearch=FindViewById<EditText>(Resource.Id.personaListTxtSearch);
            lv=FindViewById<ListView>(Resource.Id.personaListListView);

            btnAdd.Click+=delegate {
                var activityAddEdit = new Intent(this, typeof(AddEditPersonaActivity));
                StartActivity(activityAddEdit); 
            };

            btnSearch.Click+=delegate {
                LoadPersonaInList();
            };

            LoadPersonaInList();

        }

        private void LoadPersonaInList() {
            PersonaDbHelper dbVals = new PersonaDbHelper(this);
            if (txtSearch.Text.Trim().Length<1) {
                listItsms=dbVals.GetAllPersonas();
            } else {

                listItsms=dbVals.GetContactsBySearchName(txtSearch.Text.Trim());
            }

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