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
    [Activity(Label = "Info Medica List Base Adapter")]
    class InfoMedicaListBaseAdapter : BaseAdapter<InfoMedica> {
        IList<InfoMedica> infoMedicaListArrayList;
        private LayoutInflater mInflater;
        private Context activity;

        public InfoMedicaListBaseAdapter(Context context, IList<InfoMedica> results) {
            this.activity=context;
            infoMedicaListArrayList=results;
            mInflater=(LayoutInflater)activity.GetSystemService(Context.LayoutInflaterService);
        }

        public override int Count {
            get {
                return infoMedicaListArrayList.Count;
            }
        }

        public override long GetItemId(int position) {
            return position;
        }

        public override InfoMedica this[int position] {
            get {
                return infoMedicaListArrayList[position];
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent) {
            ImageView btnDelete;
            InfoMedicaViewHolder holder = null;
            if (convertView==null) {
                convertView=mInflater.Inflate(Resource.Layout.list_row_infomedica_list, null);
                holder=new InfoMedicaViewHolder();

                holder.txtIdentificacion=convertView.FindViewById<TextView>(Resource.Id.lr_identificacion);
                holder.txtPeso=convertView.FindViewById<TextView>(Resource.Id.lr_peso);
                holder.txtAltura=convertView.FindViewById<TextView>(Resource.Id.lr_altura); 
                holder.txtMasaCorporal=convertView.FindViewById<TextView>(Resource.Id.lr_masacorporal);
                holder.txtPresionArterial=convertView.FindViewById<TextView>(Resource.Id.lr_presionarterial);
                holder.txtFrecuenciaCardiaca=convertView.FindViewById<TextView>(Resource.Id.lr_frecuenciacardiaca); 
                holder.txtDetalle=convertView.FindViewById<TextView>(Resource.Id.lr_detalle);

                btnDelete=convertView.FindViewById<ImageView>(Resource.Id.lr_deleteBtn);

                btnDelete.Click+=(object sender, EventArgs e) => {
                    var poldel = (int)((sender as ImageView).Tag);
                    string id = infoMedicaListArrayList[poldel].id.ToString();
                    string identificacion = infoMedicaListArrayList[poldel].identificacion;

                    AlertDialog.Builder builder = new AlertDialog.Builder(activity);
                    AlertDialog confirm = builder.Create();
                    confirm.SetTitle("Confirmacion de borrado");
                    confirm.SetMessage("Se va a eliminar este dato: "+id+" Identificacion: "+identificacion);
                    confirm.SetButton("OK", (s, ev) => {

                        infoMedicaListArrayList.RemoveAt(poldel);

                        DeleteSelectedPersona(id);
                        NotifyDataSetChanged();

                        Toast.MakeText(activity, "Se elimino el dato", ToastLength.Short).Show();
                    });
                    confirm.SetButton2("Cancelar", (s, ev) => {

                    });

                    confirm.Show();
                };

                convertView.Tag=holder;
                btnDelete.Tag=position;
            } else {
                btnDelete=convertView.FindViewById<ImageView>(Resource.Id.lr_deleteBtn); 
                holder=convertView.Tag as InfoMedicaViewHolder;
                btnDelete.Tag=position;
            }

            double peso = Double.Parse(infoMedicaListArrayList[position].peso.ToString());
            double altura = Double.Parse(infoMedicaListArrayList[position].altura.ToString());

            double imc = Math.Round((peso/(long)Math.Pow(altura, 2)), 4);

            holder.txtIdentificacion.Text=infoMedicaListArrayList[position].identificacion;
            holder.txtAltura.Text=infoMedicaListArrayList[position].altura.ToString();
            holder.txtPeso.Text=infoMedicaListArrayList[position].peso.ToString();
            holder.txtMasaCorporal.Text=imc.ToString();
            holder.txtPresionArterial.Text=infoMedicaListArrayList[position].presionArterial.ToString();
            holder.txtFrecuenciaCardiaca.Text=infoMedicaListArrayList[position].frecuenciaCardiaca.ToString();
            holder.txtDetalle.Text=infoMedicaListArrayList[position].detalle;

            if (position%2==0) {
                convertView.SetBackgroundResource(Resource.Drawable.list_selector);
            } else {
                convertView.SetBackgroundResource(Resource.Drawable.list_selector_alternate);
            }

            return convertView;
        }

        public IList<InfoMedica> GetAllData() {
            return infoMedicaListArrayList;
        }

        public class InfoMedicaViewHolder : Java.Lang.Object {
            public TextView txtIdentificacion {
                get; set;
            }
            public TextView txtAltura {
                get; set;
            }
            public TextView txtPeso {
                get; set;
            }  
            public TextView txtMasaCorporal {
                get; set;
            }
            public TextView txtPresionArterial {
                get; set;
            }
            public TextView txtFrecuenciaCardiaca {
                get; set;
            } 
            public TextView txtDetalle {
                get; set;
            }
        }

        private void DeleteSelectedPersona(string infoMedicaId) {
            InfoMedicaDbHelper _db = new InfoMedicaDbHelper(activity);
            _db.DeleteInfoMedica(infoMedicaId);
        }
    }
}