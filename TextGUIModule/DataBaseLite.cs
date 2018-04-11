using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TextGUIModule
{
    class DataBaseLite
    {
        private string pathSQL = @"Data\mytext.db";
        private SQLiteConnection conn;
        private SQLiteCommand command;
        private int countLine = 0;
        public DataBaseLite()
        {
            if (existDataBase())
            {
                Connections();
                command = new SQLiteCommand(conn);
                isNull();
                conn.Close();
            }
            else
            {
                CreateNew();
                Connections();
                command = new SQLiteCommand(conn);
                isNull();
                conn.Close();
            }

        }
        private string textWrite(string path)
        {
            string s = "";
            using (StreamReader rw = new StreamReader(path))
            {
                while (!rw.EndOfStream)
                {
                    s += rw.ReadLine();
                    s += "\r\n";
                    countLine++;
                }
            }

            return s;
        }
        public void AddingCode(string path, string lang, string tagOnSumb)
        {
            Analitics code = new Analitics();
            code.Accect(lang, path);
            string normaList = code.GetNormalizeCode();

            string origncode = textWrite(path);
            List<string> gramsList = code.InserToDB();
            int countGram = gramsList.Count;
            string tag = DateTime.Now.ToString() + countGram.ToString();
            int indexCode = -1;
            int indexSubm = -1;
            using (command = new SQLiteCommand(
                "insert into Code (OriginCode,NormalizeCode,LengthOriginCode,CountGram, Tag ) values(@OriginCode,@NormalizeCode,@LengthOriginCode,@CountGram,@Tag)", conn))
            {
                command.Parameters.Add(new SQLiteParameter("@OriginCode", origncode));
                command.Parameters.Add(new SQLiteParameter("@NormalizeCode", normaList));
                command.Parameters.Add(new SQLiteParameter("@LengthOriginCode", countLine));
                command.Parameters.Add(new SQLiteParameter("@CountGram", countGram));
                command.Parameters.Add(new SQLiteParameter("@Tag", tag));
                command.ExecuteNonQuery();
            }
            using (command = new SQLiteCommand(
                "SELECT id from Code where Tag like @tag", conn))
            {
                command.Parameters.Add(new SQLiteParameter("@tag", tag));
                using (IDataReader r = command.ExecuteReader())
                {
                    if (r.Read())
                        indexCode = r.GetInt32(0);
                }
            }
            int gramIsTables = 0;
            int idgram = -1;
            for (int i = 0; i < gramsList.Count-1; i++)
            {
                using (command = new SQLiteCommand(
                    "SELECT COUNT(id) from Gram where Gram like @gram", conn))
                {
                    command.Parameters.Add(new SQLiteParameter("@gram", gramsList[i]));
                    using (IDataReader r = command.ExecuteReader())
                    {
                        if (r.Read())
                            gramIsTables = r.GetInt32(0);
                    }
                }

                if (gramIsTables == 0)
                {
                    using (command = new SQLiteCommand(
                        "insert into Gram (Gram) values(@Gram)", conn))
                    {
                        command.Parameters.Add(new SQLiteParameter("@Gram", gramsList[i]));
                        command.ExecuteNonQuery();
                    }
                }
                using (command = new SQLiteCommand(
                    "SELECT id from Gram where Gram like @gram", conn))
                {
                    command.Parameters.Add(new SQLiteParameter("@gram", gramsList[i]));
                    using (IDataReader r = command.ExecuteReader())
                    {
                        if (r.Read())
                            idgram = r.GetInt32(0);
                    }
                }
                using (command = new SQLiteCommand(
                    "insert into KGrmams (id_Gram,id_Code) values(@id_Gram,@id_Code)", conn))
                {
                    command.Parameters.Add(new SQLiteParameter("@id_Gram", idgram));
                    command.Parameters.Add(new SQLiteParameter("@id_Code", indexCode));
                    command.ExecuteNonQuery();
                }
            }
            using (command = new SQLiteCommand(
                "SELECT id from Submit where Tag like @Tag", conn))
            {
                command.Parameters.Add(new SQLiteParameter("@Tag", tagOnSumb));
                using (IDataReader r = command.ExecuteReader())
                {
                    if (r.Read())
                        indexSubm = r.GetInt32(0);
                }
            }

            using (command = new SQLiteCommand(
                "insert into File (Name,Ver,Hash,id_Submit, id_Code ) values(@Name,@Ver,@Hash,@id_Submit, @id_Code )", conn))
            {
                command.Parameters.Add(new SQLiteParameter("@Name", code.FileName(path)));
                command.Parameters.Add(new SQLiteParameter("@Ver", code.GetVersion(path)));
                command.Parameters.Add(new SQLiteParameter("@Hash", code.GetHash(path)));
                command.Parameters.Add(new SQLiteParameter("@id_Submit", indexSubm));
                command.Parameters.Add(new SQLiteParameter("@id_Code", indexCode));
                command.ExecuteNonQuery();
            }
        }
        
        /*------------------Submit---------------------*/
        public void AddingSubmit(string name, string desc,string compolType, string path)
        {
            conn.Open();
            //command.CommandText = String.Format("SELECT COUNT(*) from User where Name like '{0}')", name);
            string language = "";
            int amount = 0;
            int indexUser = 0;
            int indexCompil = 0;
            string tahSubmit = "";
            try
            {
                using (command = new SQLiteCommand(
                      "SELECT COUNT(id) from User where Name like @name", conn))
                {
                    command.Parameters.Add(new SQLiteParameter("@name",name));
                    using (IDataReader r = command.ExecuteReader())
                    {
                        if (r.Read())
                            amount = r.GetInt32(0);
                    }
                }
                
                if (amount == 0)
                {
                    using (command = new SQLiteCommand(
                           "insert into User (Name) values(@Name)", conn))
                    {
                        command.Parameters.Add(new SQLiteParameter("@Name",name));
                        command.ExecuteNonQuery();
                    }
                }

                using (command = new SQLiteCommand(
                    "SELECT id from User where Name like @name", conn))
                {
                    command.Parameters.Add(new SQLiteParameter("@name", name));
                    using (IDataReader r = command.ExecuteReader())
                    {
                        if (r.Read())
                            indexUser = r.GetInt32(0);
                    }
                }
                using (command = new SQLiteCommand(
                    "SELECT id from Compiler where CompilerType like @copmType", conn))
                {
                    command.Parameters.Add(new SQLiteParameter("@copmType", compolType));
                    using (IDataReader r = command.ExecuteReader())
                    {
                        if (r.Read())
                            indexCompil = r.GetInt32(0);
                    }
                }
                using (command = new SQLiteCommand(
                    "insert into Submit (Tag, description, DateTimeSend, id_Compiler, id_User) values(@Tag,@dect,@Data,@indComp,@indUse)", conn))
                {
                    string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    tahSubmit = name + time;
                    command.Parameters.Add(new SQLiteParameter("@dect", desc));
                    command.Parameters.Add(new SQLiteParameter("@Data", time));
                    command.Parameters.Add(new SQLiteParameter("@indComp", indexCompil));
                    command.Parameters.Add(new SQLiteParameter("@indUse", indexUser));
                    command.Parameters.Add(new SQLiteParameter("@Tag", tahSubmit));
                    command.ExecuteNonQuery();
                }
                using (command = new SQLiteCommand(
                    "SELECT denomination from Language join Compiler on Compiler.id_language = Language.id where CompilerType like @copmType", conn))
                {
                    command.Parameters.Add(new SQLiteParameter("@copmType", compolType));
                    using (IDataReader r = command.ExecuteReader())
                    {
                        if (r.Read())
                            language = r.GetString(0);
                    }
                }

            }
            catch (SQLiteException ex)
                {
                    prontError(ex.Message);
                }

            
            AddingCode(path,language, tahSubmit);
            conn.Close();
        }

        public List<string> GetCompile(string lang)
        {
            int idlang = -1;
            conn.Open();
            List<string> compList = new List<string>();
            using (command = new SQLiteCommand(
                "SELECT id from Language where denomination like @lang", conn))
            {
                command.Parameters.Add(new SQLiteParameter("@lang", lang));
                using (IDataReader r = command.ExecuteReader())
                {
                    if (r.Read())
                        idlang = r.GetInt32(0);
                }
            }
            using (command = new SQLiteCommand(
                "SELECT CompilerType from Compiler where id_language = @id_lg", conn))
            {
                command.Parameters.Add(new SQLiteParameter("@id_lg", idlang));
                using (IDataReader r = command.ExecuteReader())
                {
                    if (r.Read())
                        compList.Add(r.GetString(0));
                }
            }
            conn.Close();
            return compList;
        }
        /*------------------Submit---------------------*/

        /*------------Grams---------------*/
        public void AddingGrams(string s)
        {
            if (Connections())
            {
                command.CommandText = "SELECT EXISTS(SELECT id FROM Gram WHERE Gram = '')";
                object amount = false;
                try
                {
                    amount = command.ExecuteScalar();
                }
                catch (SQLiteException ex)
                {
                    prontError(ex.Message);
                }
                
            }
            
        }
        /*------------Grams---------------*/
        private void CreateNew()
        {
            SQLiteConnection.CreateFile(pathSQL);
        }

        private bool Connections()
        {
            conn = new SQLiteConnection(String.Format("Data Source={0}; Version=3;", pathSQL));
            try
            {
                conn.Open();
                prontInfo("Connect!");
                return true;
                

            }
            catch (Exception ex)
            {
                prontError(ex.Message);
                return false;
            }
        }

        private void standCreateTables()
        {
            string createLanguage = "CREATE TABLE Language ("
                                + "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,"
                                + "denomination TEXT NOT NULL"
                                + " ); ";
            string createCompoler = "CREATE TABLE Compiler ("
                                   + "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,"
                                   + "FullName	TEXT NOT NULL,"
                                   + "CompilerType	TEXT NOT NULL,"
                                   + "id_language INTEGER NOT NULL,"
                                   + "FOREIGN KEY(id_language) REFERENCES Language(id)"
                                   + "); ";
            string createCode = "CREATE TABLE Code ("
                                + "id	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,"
                                + "OriginCode	TEXT NOT NULL,"
                                + "NormalizeCode	TEXT NOT NULL,"
                                + "LengthOriginCode	INTEGER NOT NULL,"
                                + "CountGram	INTEGER NOT NULL,"
                                + "Tag	TEXT NOT NULL UNIQUE"
                                + "); ";
            string createGram = "CREATE TABLE Gram ("
                                 + "id	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,"
                                 + "Gram	TEXT NOT NULL"
                                 + "); ";
            string createKGrams = "CREATE TABLE KGrmams ("
                                  + "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,"
                                  + "id_Gram	INTEGER NOT NULL,"
                                  + "id_Code	INTEGER,"
                                  + " FOREIGN KEY(id_Gram) REFERENCES Gram(id)"
                                  + " ); ";
            string createUser = "CREATE TABLE User ("
                                + "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,"
                                + "Name	TEXT NOT NULL"
                                + "); ";
            string createSubmite = "CREATE TABLE Submit ("
                                   + "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,"
                                   + "description	TEXT,"
                                   + "DateTimeSend	NUMERIC NOT NULL,"
                                   + "id_Compiler	INTEGER NOT NULL,"
                                   + "id_User	INTEGER NOT NULL,"
                                   + "Tag TEXT NOT NULL UNIQUE,"
                                   + "FOREIGN KEY(id_User) REFERENCES User(id),"
                                   + "FOREIGN KEY(id_Compiler) REFERENCES Compiler(id)"
                                   + " ); ";
            string createHistory = "CREATE TABLE History ("
                                   + "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,"
                                   + "Date NUMERIC NOT NULL,"
                                   + "State_VShingl	REAL NOT NULL,"
                                   + "State_DistenceLiv	REAL NOT NULL,"
                                   + "State_Heskela	REAL NOT NULL,"
                                   + "id_Sublit1 INTEGER NOT NULL,"
                                   + "id_Submit2 INTEGER NOT NULL,"
                                   + "FOREIGN KEY(id_Submit2) REFERENCES Submit(id),"
                                   + "FOREIGN KEY(id_Sublit1) REFERENCES Submit(id)"
                                   + "); ";
            string createFile = "CREATE TABLE File ("
                                + "id	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,"
                                + "Name	TEXT NOT NULL,"
                                + "Ver	INTEGER,"
                                + "Hash	INTEGER NOT NULL,"
                                + "id_Submit	INTEGER NOT NULL,"
                                + "id_Code	INTEGER NOT NULL,"
                                + "FOREIGN KEY(id_Submit) REFERENCES Submit(id)"
                                + "); ";
            string createFormate = "CREATE TABLE Format ("
                                   + "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,"
                                   + "FormatTag	TEXT NOT NULL,"
                                   + "id_Language	INTEGER NOT NULL,"
                                   + "id_file	INTEGER NOT NULL,"
                                   + " FOREIGN KEY(id_Language) REFERENCES Language(id),"
                                   + "FOREIGN KEY(id_file) REFERENCES File(id)"
                                   + "); ";

            initTable(createLanguage);
            initTable(createCompoler);
            initTable(createCode);
            initTable(createGram);
            initTable(createKGrams);
            initTable(createUser);
            initTable(createSubmite);
            initTable(createHistory);
            initTable(createFile);
            initTable(createFormate);
            defoultValue();
            prontInfo("Creare new DataBase!");
        }

        private void defoultValue()
        {
            string defLang = "insert into Language (denomination)"
                             + "values('C#'),"
                             + "('Java'), "
                             + "('C'), "
                             + "('C++'); ";
            initTable(defLang);
            string defCompil = "insert into Compiler (FullName,CompilerType,id_language)"
                               + "values('Comol for C#. Version 2.1', 'dotnet',1),"
                               + "('Comol for Java. Version 2.1', 'javavm',2), "
                               + "('Comol for C. Version 2.0', 'custom',3), "
                               + "('Comol for C+. Version 2.1', 'native',4); ";
            initTable(defCompil);

        }

        private void initTable(string commands)
        {
            command.CommandText = commands;
            try
            {
                command.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                 prontError(ex.Message);
            }
        }
        private bool isNull()
        {
            //command = new SQLiteCommand(conn);
            //command.CommandText = "SELECT sum(name) FROM sqlite_master WHERE type = 'table';";
            //var allTableData = command.ExecuteReader();
            DataTable allTableData = conn.GetSchema("Tables");
            if (allTableData.Rows.Count > 0)
            {
                return false;
            }
            else
            {
                standCreateTables();
                return true;
            }
        }
        private bool existDataBase()
        {
            return (File.Exists(pathSQL));
        }

        private void prontInfo(string mess)
        {
            MessageBox.Show(mess, "EvolPras", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void prontError(string mess)
        {
            MessageBox.Show(mess, "Exeptions", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
