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
    class PersonaDbHelper : SQLiteOpenHelper {
        private const string APP_DATABASENAME = "Project22a.db3";
        private const int APP_DATABASE_VERSION = 1;

        public PersonaDbHelper(Context ctx) :
            base(ctx, APP_DATABASENAME, null, APP_DATABASE_VERSION) {
        }

        public override void OnCreate(SQLiteDatabase db) {

            db.ExecSQL(@"CREATE TABLE IF NOT EXISTS InfoMedica(
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            idPersona TEXT NOT NULL,
                            identificacion TEXT NOT NULL,
                            peso TEXT NOT NULL,
                            altura TEXT NOT NULL,
                            presionArterial TEXT,
                            frecuenciaCardiaca TEXT,
                            detalle TEXT)");

            db.ExecSQL("Insert into InfoMedica(idPersona, identificacion, peso, altura, presionArterial, frecuenciaCardiaca, detalle) "+
                "VALUES('1', '112850706','65','165','180-60','150', 'Algun detalle')");

            db.ExecSQL("Insert into InfoMedica(idPersona, identificacion, peso, altura, presionArterial, frecuenciaCardiaca, detalle) "+
                "VALUES('1', '112850706','74','165','181-60','151', 'Algun detalle nuevo')");

            db.ExecSQL(@"CREATE TABLE IF NOT EXISTS Persona(
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            identificacion TEXT NOT NULL,
                            nombre TEXT NOT NULL,
                            apellidos TEXT NOT NULL,
                            direccion TEXT,
                            correo TEXT,
                            telefono TEXT,
                            fechaNac TEXT,
                            sexo TEXT,
                            estadoCivil TEXT,
                            tipoSangre TEXT,
                            detalle TEXT)");

            db.ExecSQL("Insert into Persona(identificacion, nombre, apellidos, direccion, correo, telefono, fechaNac, sexo, estadoCivil, tipoSangre, detalle) "+
                "VALUES('112850706','Albin','Sandi Gamboa','Heredia','asandi01@gmail.com','88939033','10/07/1986', 'Hombre', 'Casado', 'O+', 'Algun detalle')");

            db.ExecSQL("Insert into Persona(identificacion, nombre, apellidos, direccion, correo, telefono, fechaNac, sexo, estadoCivil, tipoSangre, detalle) "+
                "VALUES('11223344','Example Name','Example Apellido','Heredia','asandi02@gmail.com','88939033','10/07/1986', 'Hombre', 'Casado', 'O-', 'Algun detalle')");

        }

        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
            db.ExecSQL("DROP TABLE IF EXISTS Persona");
            OnCreate(db);
        }

        //Retrive All Contact Details
        public IList<Persona> GetAllPersonas() {
            SQLiteDatabase db = this.ReadableDatabase; 
            ICursor c = db.Query("Persona", new string[] { "id", "identificacion", "nombre", "apellidos", "direccion", "correo", "telefono", "fechaNac", "sexo", "estadoCivil", "tipoSangre", "detalle" }, null, null, null, null, null);

            var personas = new List<Persona>();

            while (c.MoveToNext()) {
                personas.Add(new Persona {
                    id=c.GetInt(0),
                    identificacion=c.GetString(1),
                    nombre=c.GetString(2),
                    apellidos=c.GetString(3),
                    direccion=c.GetString(4),
                    correo=c.GetString(5),
                    telefono=c.GetString(6),
                    fechaNac=Convert.ToDateTime(c.GetString(7)),
                    sexo=c.GetString(8),
                    estadoCivil=c.GetString(9),
                    tipoSangre=c.GetString(10),
                    detalle=c.GetString(11)
                });
            }

            c.Close();
            db.Close();
            return personas;
        }


        //Retrive All Contact Details
        public IList<Persona> GetContactsBySearchName(string nameToSearch) {
            SQLiteDatabase db = this.ReadableDatabase;   

            ICursor c = db.Query("Persona", new string[] { "id", "identificacion", "nombre", "apellidos", "direccion", "correo", "telefono", "fechaNac", "sexo", "estadoCivil", "tipoSangre", "detalle" }, "upper(nombre) LIKE ?", new string[] { "%"+nameToSearch.ToUpper()+"%" }, null, null, null, null);

            var contacts = new List<Persona>();

            while (c.MoveToNext()) {
                contacts.Add(new Persona {
                    id=c.GetInt(0),
                    identificacion=c.GetString(1),
                    nombre=c.GetString(2),
                    apellidos=c.GetString(3),
                    direccion=c.GetString(4),
                    correo=c.GetString(5),
                    telefono=c.GetString(6),
                    fechaNac=Convert.ToDateTime(c.GetString(7)),
                    sexo=c.GetString(8),
                    estadoCivil=c.GetString(9),
                    tipoSangre=c.GetString(10),
                    detalle=c.GetString(11)
                });
            }

            c.Close();
            db.Close();

            return contacts;
        }

        //Add New Contact
        public void AddNewPersona(Persona personainfo) {
            SQLiteDatabase db = this.WritableDatabase;
            ContentValues vals = new ContentValues();

            vals.Put("identificacion", personainfo.identificacion);
            vals.Put("nombre", personainfo.nombre);
            vals.Put("apellidos", personainfo.apellidos);
            vals.Put("direccion", personainfo.direccion);
            vals.Put("correo", personainfo.correo);
            vals.Put("telefono", personainfo.telefono);
            vals.Put("fechaNac", personainfo.fechaNac.ToString());
            vals.Put("sexo", personainfo.sexo);
            vals.Put("estadoCivil", personainfo.estadoCivil);
            vals.Put("tipoSangre", personainfo.tipoSangre);
            vals.Put("detalle", personainfo.detalle);

            db.Insert("Persona", null, vals);
        }

        //Get persona details by Id
        public ICursor getPersonaById(int id) {
            SQLiteDatabase db = this.ReadableDatabase;
            ICursor res = db.RawQuery("select * from Persona where id="+id+"", null);
            return res;
        }

        //Update Existing contact
        public void UpdatePersona(Persona personainfo) {
            if (personainfo==null) {
                return;
            }

            //Obtain writable database
            SQLiteDatabase db = this.WritableDatabase;

            //Prepare content values
            ContentValues vals = new ContentValues();
            vals.Put("identificacion", personainfo.identificacion);
            vals.Put("nombre", personainfo.nombre);
            vals.Put("apellidos", personainfo.apellidos);
            vals.Put("direccion", personainfo.direccion);
            vals.Put("correo", personainfo.correo);
            vals.Put("telefono", personainfo.telefono);
            vals.Put("fechaNac", personainfo.fechaNac.ToString());
            vals.Put("sexo", personainfo.sexo);
            vals.Put("estadoCivil", personainfo.estadoCivil);
            vals.Put("tipoSangre", personainfo.tipoSangre);
            vals.Put("detalle", personainfo.detalle); 

            ICursor cursor = db.Query("Persona",
                    new String[] { "id", "identificacion", "nombre", "apellidos", "direccion", "correo", "telefono", "fechaNac", "sexo", "estadoCivil", "tipoSangre", "detalle" }, "id=?", new string[] { personainfo.id.ToString() }, null, null, null, null);
                       
            if (cursor!=null) {
                if (cursor.MoveToFirst()) {
                    // update the row
                    db.Update("Persona", vals, "id=?", new String[] { cursor.GetString(0) });
                }

                cursor.Close();
            }

        }

        //Delete Existing contact
        public void DeletePersona(string personaId) {
            if (personaId ==null) {
                return;
            }

            //Obtain writable database
            SQLiteDatabase db = this.WritableDatabase;  

            ICursor cursor = db.Query("Persona",
                    new String[] { "id", "identificacion", "nombre", "apellidos", "direccion", "correo", "telefono", "fechaNac", "sexo", "estadoCivil", "tipoSangre", "detalle" }, "id=?", new string[] { personaId }, null, null, null, null);

            if (cursor!=null) {
                if (cursor.MoveToFirst()) {
                    // update the row
                    db.Delete("Persona", "id=?", new String[] { cursor.GetString(0) });
                }

                cursor.Close();
            }

        }

    }
}