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
using Android.Database.Sqlite;
using Android.Database;

namespace Project22 {
    class InfoMedicaDbHelper : SQLiteOpenHelper {
        private const string APP_DATABASENAME = "Project22a.db3";
        private const int APP_DATABASE_VERSION = 1;

        public InfoMedicaDbHelper(Context ctx) :
            base(ctx, APP_DATABASENAME, null, APP_DATABASE_VERSION) {
        }

        public override void OnCreate(SQLiteDatabase db) {
            db.ExecSQL(@"CREATE TABLE IF NOT EXISTS InfoMedica(
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            idPersona TEXT NOT NULL,
                            identificacion TEXT NOT NULL,
                            peso TEXT NOT NULL,
                            altura TEXT NOT NULL,
                            presionArterial TEXT,
                            frecuenciaCardiaca TEXT)");

            db.ExecSQL("Insert into InfoMedica(idPersona, identificacion, peso, altura, presionArterial, frecuenciaCardiaca, detalle) "+
                "VALUES(1, '112850706','65','165','180-60','150', 'Algun detalle')");

            db.ExecSQL("Insert into InfoMedica(idPersona, identificacion, peso, altura, presionArterial, frecuenciaCardiaca, detalle) "+
                "VALUES(1, '112850706','74','165','181-60','151', 'Algun detalle nuevo')");

        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
            db.ExecSQL("DROP TABLE IF EXISTS InfoMedica");
            OnCreate(db);
        }

        //Retrive All Details
        public IList<InfoMedica> GetAllInfoMedica(string idPersonal) {     
            SQLiteDatabase db = this.ReadableDatabase; 
            ICursor c = db.Query("InfoMedica", new string[] { "id", "idPersona", "identificacion", "peso", "altura", "presionArterial", "frecuenciaCardiaca", "detalle" }, "idPersona =  "+idPersonal, null, null, null, null);

            var infoMedica = new List<InfoMedica>();

            while (c.MoveToNext()) {
                infoMedica.Add(new InfoMedica {
                    id=c.GetInt(0),
                    idPersona=Int32.Parse(c.GetString(1)),
                    identificacion=c.GetString(2),
                    peso=c.GetDouble(3),
                    altura=c.GetDouble(4),
                    presionArterial=c.GetString(5),
                    frecuenciaCardiaca=c.GetString(6),
                    detalle=c.GetString(7)
                });
            }

            c.Close();
            db.Close();
            return infoMedica;
        }

        //Add New Contact
        public void AddNewInfoMedica(InfoMedica infoMedica) {
            SQLiteDatabase db = this.WritableDatabase;
            ContentValues vals = new ContentValues();
                                                         
            vals.Put("idPersona", infoMedica.idPersona);
            vals.Put("identificacion", infoMedica.identificacion);
            vals.Put("peso", infoMedica.peso);
            vals.Put("altura", infoMedica.altura);
            vals.Put("presionArterial", infoMedica.presionArterial);
            vals.Put("frecuenciaCardiaca", infoMedica.frecuenciaCardiaca);
            vals.Put("detalle", infoMedica.detalle);

            db.Insert("InfoMedica", null, vals);
        }

        //Get details by Id
        public ICursor getInfoMedicaById(int id) {
            SQLiteDatabase db = this.ReadableDatabase;
            ICursor res = db.RawQuery("select * from InfoMedica where id="+id+"", null);
            return res;
        }

        //Update Existing contact
        public void UpdateInfoMedica(InfoMedica infoMedica) {
            if (infoMedica==null) {
                return;
            }

            //Obtain writable database
            SQLiteDatabase db = this.WritableDatabase;

            //Prepare content values
            ContentValues vals = new ContentValues();
            vals.Put("idPersona", infoMedica.idPersona);
            vals.Put("identificacion", infoMedica.identificacion);
            vals.Put("peso", infoMedica.peso);
            vals.Put("altura", infoMedica.altura);
            vals.Put("presionArterial", infoMedica.presionArterial);
            vals.Put("frecuenciaCardiaca", infoMedica.frecuenciaCardiaca);
            vals.Put("detalle", infoMedica.detalle);

            ICursor cursor = db.Query("InfoMedica",
                    new String[] { "id", "idPersona", "identificacion", "peso", "altura", "presionArterial", "frecuenciaCardiaca", "detalle" }, "id=?", new string[] { infoMedica.id.ToString() }, null, null, null, null);
                       
            if (cursor!=null) {
                if (cursor.MoveToFirst()) {
                    // update the row
                    db.Update("InfoMedica", vals, "id=?", new String[] { cursor.GetString(0) });
                }

                cursor.Close();
            }

        }

        //Delete Existing contact
        public void DeleteInfoMedica(string infoMedicaId) {
            if (infoMedicaId==null) {
                return;
            }

            //Obtain writable database
            SQLiteDatabase db = this.WritableDatabase;  

            ICursor cursor = db.Query("InfoMedica",
                    new String[] { "id", "idPersona", "identificacion", "peso", "altura", "presionArterial", "frecuenciaCardiaca", "detalle" }, "id=?", new string[] { infoMedicaId }, null, null, null, null);

            if (cursor!=null) {
                if (cursor.MoveToFirst()) {
                    // update the row
                    db.Delete("InfoMedica", "id=?", new String[] { cursor.GetString(0) });
                }

                cursor.Close();
            }
        }

    }
}