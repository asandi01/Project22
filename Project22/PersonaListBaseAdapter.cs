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
    [Activity(Label = "Persona List Base Adapter")]
    class PersonaListBaseAdapter : BaseAdapter<Persona> {
        IList<Persona> personaListArrayList;
        private LayoutInflater mInflater;
        private Context activity;

        public PersonaListBaseAdapter(Context context, IList<Persona> results) {
            this.activity=context;
            personaListArrayList=results;
            mInflater=(LayoutInflater)activity.GetSystemService(Context.LayoutInflaterService);
        }

        public override int Count {
            get {
                return personaListArrayList.Count;
            }
        }

        public override long GetItemId(int position) {
            return position;
        }

        public override Persona this[int position] {
            get {
                return personaListArrayList[position];
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent) {
            ImageView btnDelete, btnView;
            PersonaViewHolder holder = null;
            if (convertView==null) {
                convertView=mInflater.Inflate(Resource.Layout.list_row_persona_list, null);
                holder=new PersonaViewHolder();

                holder.txtIdentificacion=convertView.FindViewById<TextView>(Resource.Id.lr_email);
                holder.txtNombre=convertView.FindViewById<TextView>(Resource.Id.lr_fullName);
                holder.txtApellidos=convertView.FindViewById<TextView>(Resource.Id.lr_mobile);
                holder.txtDetalle=convertView.FindViewById<TextView>(Resource.Id.lr_descriptin);
                holder.txtEdad=convertView.FindViewById<TextView>(Resource.Id.lr_eddad);

                btnDelete=convertView.FindViewById<ImageView>(Resource.Id.lr_deleteBtn);
                btnView=convertView.FindViewById<ImageView>(Resource.Id.lr_viewBtn);




                btnView.Click+=(object sender, EventArgs e) => {
                    var poldel = (int)((sender as ImageView).Tag);

                    string id = personaListArrayList[poldel].id.ToString();
                    string nombre = personaListArrayList[poldel].nombre.ToString();
                    string identificacion = personaListArrayList[poldel].identificacion.ToString();

                    var activity2 = new Intent(activity, typeof(InfoMedicaActivity));
                    activity2.PutExtra("PersonaId", personaListArrayList[poldel].id.ToString());
                    activity2.PutExtra("PersonaNombre", personaListArrayList[poldel].nombre.ToString());
                    activity2.PutExtra("PersonaIdentificacion", personaListArrayList[poldel].identificacion.ToString());
                    activity.StartActivity(activity2);
                    Toast.MakeText(activity, "Ver persona: "+id+" "+nombre+" "+identificacion, ToastLength.Short).Show();
                };


                btnDelete.Click+=(object sender, EventArgs e) => {
                    var poldel = (int)((sender as ImageView).Tag);
                    string id = personaListArrayList[poldel].id.ToString();
                    string fname = personaListArrayList[poldel].nombre;

                    AlertDialog.Builder builder = new AlertDialog.Builder(activity);
                    AlertDialog confirm = builder.Create();
                    confirm.SetTitle("Confirmacion de borrado");
                    confirm.SetMessage("Se va a eliminar esta perona: "+id+" Nombre: "+fname);
                    confirm.SetButton("OK", (s, ev) => {

                        personaListArrayList.RemoveAt(poldel);

                        DeleteSelectedPersona(id);
                        NotifyDataSetChanged();

                        Toast.MakeText(activity, "Se elimino la persona", ToastLength.Short).Show();
                    });
                    confirm.SetButton2("Cancelar", (s, ev) => {

                    });

                    confirm.Show();
                };

                convertView.Tag=holder;
                btnDelete.Tag=position;
                btnView.Tag=position;
            } else {
                btnDelete=convertView.FindViewById<ImageView>(Resource.Id.lr_deleteBtn);
                btnView=convertView.FindViewById<ImageView>(Resource.Id.lr_viewBtn);
                holder=convertView.Tag as PersonaViewHolder;
                btnDelete.Tag=position;
                btnView.Tag=position;
            }
                                                 
            DateTime nacimiento = new DateTime(Convert.ToDateTime(personaListArrayList[position].fechaNac).Year, Convert.ToDateTime(personaListArrayList[position].fechaNac).Month, Convert.ToDateTime(personaListArrayList[position].fechaNac).Day);
            int edad = DateTime.Today.AddTicks(-nacimiento.Ticks).Year-1;
            string edadString = "";
            if (edad<200) {
                edadString=edad.ToString();
            }

            holder.txtIdentificacion.Text=personaListArrayList[position].identificacion;
            holder.txtNombre.Text=personaListArrayList[position].nombre.ToString();
            holder.txtApellidos.Text=personaListArrayList[position].apellidos;
            holder.txtDetalle.Text=personaListArrayList[position].detalle;
            holder.txtEdad.Text=edadString;

            if (position%2==0) {
                convertView.SetBackgroundResource(Resource.Drawable.list_selector);
            } else {
                convertView.SetBackgroundResource(Resource.Drawable.list_selector_alternate);
            }

            return convertView;
        }

        public IList<Persona> GetAllData() {
            return personaListArrayList;
        }

        public class PersonaViewHolder : Java.Lang.Object {
            public TextView txtIdentificacion {
                get; set;
            }
            public TextView txtNombre {
                get; set;
            }
            public TextView txtApellidos {
                get; set;
            }
            public TextView txtDetalle {
                get; set;
            }
            public TextView txtEdad {
                get; set;
            }
        }

        private void DeleteSelectedPersona(string contactId) {
            PersonaDbHelper _db = new PersonaDbHelper(activity);
            _db.DeletePersona(contactId);
        }
    }
}