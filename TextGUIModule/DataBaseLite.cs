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
        private int idiDenticalFie = -1;
        public int IdiDenticalFie
        {
            get
            {
                return idiDenticalFie;
            }
        }
        public DataBaseLite()
        {
            if (ExistDataBase())
            {
                Connections();
                command = new SQLiteCommand(conn);
                IsNull();
                conn.Close();
            }
            else
            {
                CreateNew();
                Connections();
                command = new SQLiteCommand(conn);
                IsNull();
                conn.Close();
            }

        }
        private string TextWrite(string path)
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

        public void SearchIn(string tagAddingSub)
        {
            conn.Open();
            List<string> gram = new List<string>();
            Dictionary<int,string> nameFile = new Dictionary<int, string>();
            Dictionary<int, double > resCode = new Dictionary<int, double>();
            Analitics search = new Analitics();
            bool isDel = false;
            int idMainFileFor = -1;
            string mainCode = "";
            string childCode = "";
            double maxRes = Double.MinValue; 
            using (command = new SQLiteCommand("select File.id from File join Submit on File.id_Submit = Submit.id where Submit.Tag = @tag", conn))
            {
                command.Parameters.Add(new SQLiteParameter("@Tag", tagAddingSub));
                using (IDataReader r = command.ExecuteReader())
                {
                    if (r.Read())
                    {
                        idMainFileFor = r.GetInt32(0);
                    }

                }
            }

            using (command = new SQLiteCommand(
                "select Gram.Gram from Gram join KGrmams on KGrmams.id_Gram = Gram.id join Code on KGrmams.id_Code = Code.id join File on File.id_Code = Code.id where File.id = @id", conn))
            {
                command.Parameters.Add(new SQLiteParameter("@id", idMainFileFor));
                using (IDataReader r = command.ExecuteReader())
                {
                    while (r.Read())
                    {
                        gram.Add(r.GetString(0));
                    }

                }
            }


            for (int i = 0; i < gram.Count/2; i++)
            {
                using (command = new SQLiteCommand(
                    "select DISTINCT File.Name, File.id from File join Code on File.id_Code = Code.id join KGrmams on KGrmams.id_Code = Code.id join Gram on KGrmams.id_Gram = Gram.id where Gram.Gram = @gram and File.id != @idFile", conn))
                {

                    command.Parameters.Add(new SQLiteParameter("@gram", gram[i]));
                    command.Parameters.Add(new SQLiteParameter("@idFile", idMainFileFor));
                    using (IDataReader r = command.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            if (!nameFile.ContainsKey(r.GetInt32(1)) && isDel && nameFile.Count > 1)
                            {
                                nameFile.Remove(r.GetInt32(1));
                            }
                            else if (!isDel)
                            {
                                nameFile.Add(r.GetInt32(1),r.GetString(0));
                            }
                            
                        }
                           
                    }
                }
                isDel = true;
            }
            using (command = new SQLiteCommand(
                "select Code.NormalizeCode from Code join File on File.id_Code = Code.id where File.id = @id", conn))
            {
                command.Parameters.Add(new SQLiteParameter("@id", idMainFileFor));
                using (IDataReader r = command.ExecuteReader())
                {
                    if (r.Read())
                        mainCode += r.GetString(0);
                }
            }
            search.SetCodeMain(mainCode);
            foreach (int key in nameFile.Keys)
            {
                using (command = new SQLiteCommand(
                    "select Code.NormalizeCode from Code join File on File.id_Code = Code.id where File.id = @id", conn))
                {
                    command.Parameters.Add(new SQLiteParameter("@id", key));
                    using (IDataReader r = command.ExecuteReader())
                    {
                        if (r.Read())
                            childCode += r.GetString(0);
                    }
                }
                search.SetCodeChild(childCode);
                resCode.Add(key,search.AlgHeskel());
            }

            foreach (int Key in resCode.Keys)
            {
                if (resCode[Key] > maxRes)
                {
                    maxRes = resCode[Key];
                    idiDenticalFie = Key;
                }
                    
                    
            }
            conn.Close();
        }

        public List<string> DescSubm()
        {
            List<string> allDesc = new List<string>();
            conn.Open();
            string s = "";
            using (command = new SQLiteCommand(
                "SELECT File.Name, User.Name, Submit.Tag from User join Submit on Submit.id_User = User.id join File on File.id_Submit = Submit.id;", conn))
            {
                
                using (IDataReader r = command.ExecuteReader())
                {
                    while (r.Read())
                    {
                        s += r.GetString(0);
                        s += "|";
                        s += r.GetString(1);
                        s += "|";
                        s += r.GetString(2);
                        allDesc.Add(s);
                        s = "";
                    }
                        
                }
            }
            conn.Close();
            return allDesc;
        }
        public void SetCodeMain(string tag, Analitics alg)
        {
            string tokins = GetTokingCode(tag);
            alg.SetCodeMain(tokins);

        }
        public void SetCodeChild(int id, Analitics alg)
        {
            string tagSubmit = "";
            conn.Open();
            using (command = new SQLiteCommand(
                "select Submit.Tag from Submit join File on File.id_Submit = Submit.id where File.id = @id", conn))
            {
                command.Parameters.Add(new SQLiteParameter("@id", idiDenticalFie));
                using (IDataReader r = command.ExecuteReader())
                {
                    if (r.Read())
                        tagSubmit += r.GetString(0);
                }
            }

            conn.Close();
            SetCodeChild(tagSubmit, alg);
        }
        public void SetCodeChild(string tag, Analitics alg)
        {
            string tokins = GetTokingCode(tag);
            alg.SetCodeChild(tokins);

        }
        private string GetTokingCode(string tag)
        {
            string originCode = "";
            conn.Open();
            using (command = new SQLiteCommand(
                "SELECT Code.NormalizeCode from Code join File on File.id_Code = Code.id join Submit on File.id_Submit = Submit.id where Submit.Tag like @Tag;", conn))
            {
                command.Parameters.Add(new SQLiteParameter("@Tag", tag));
                using (IDataReader r = command.ExecuteReader())
                {
                    if (r.Read())
                        originCode += r.GetString(0);
                }
            }
            conn.Close();
            return originCode;
        }

        public string GetOrignCodeFromId(int id)
        {
            string tagSubmit = "";
            conn.Open();
            using (command = new SQLiteCommand(
                "select Submit.Tag from Submit join File on File.id_Submit = Submit.id where File.id = @id", conn))
            {
                command.Parameters.Add(new SQLiteParameter("@id", idiDenticalFie));
                using (IDataReader r = command.ExecuteReader())
                {
                    if (r.Read())
                        tagSubmit += r.GetString(0);
                }
            }
            conn.Close();
            return GetOrignCode(tagSubmit);
        }
        public string GetOrignCode(string tag)
        {
            string originCode = "";
            conn.Open();
            using (command = new SQLiteCommand(
                "SELECT Code.OriginCode from Code join File on File.id_Code = Code.id join Submit on File.id_Submit = Submit.id where Submit.Tag like @Tag;", conn))
            {
                command.Parameters.Add(new SQLiteParameter("@Tag", tag));
                using (IDataReader r = command.ExecuteReader())
                {
                    if (r.Read())
                        originCode += r.GetString(0);
                }
            }
            conn.Close();
            return originCode;
        }
        /*------------------Code---------------------*/
        private void AddingCode(string path, string lang, string tagOnSumb, bool serachNow)
        {
            Analitics code = new Analitics();
            code.Accect(lang, path);
            string normaList = code.GetNormalizeCode();

            string origncode = TextWrite(path);
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

            if (serachNow)
            {
                SearchIn(tagOnSumb);
            }
        }
        /*------------------Code---------------------*/
        /*------------------Submit---------------------*/
        public void AddingSubmit(string name, string desc,string compolType, string path, bool serachNow)
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
                    ProntError(ex.Message);
                }

            
            AddingCode(path,language, tahSubmit, serachNow);
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
                    ProntError(ex.Message);
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
                ProntInfo("Connect!");
                return true;
                

            }
            catch (Exception ex)
            {
                ProntError(ex.Message);
                return false;
            }
        }

        private void StandCreateTables()
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

            InitTable(createLanguage);
            InitTable(createCompoler);
            InitTable(createCode);
            InitTable(createGram);
            InitTable(createKGrams);
            InitTable(createUser);
            InitTable(createSubmite);
            InitTable(createHistory);
            InitTable(createFile);
            InitTable(createFormate);
            DefoultValue();
            ProntInfo("Creare new DataBase!");
        }

        private void DefoultValue()
        {
            string defLang = "insert into Language (denomination)"
                             + "values('C#'),"
                             + "('Java'), "
                             + "('C'), "
                             + "('C++'); ";
            InitTable(defLang);
            string defCompil = "insert into Compiler (FullName,CompilerType,id_language)"
                               + "values('Comol for C#. Version 2.1', 'dotnet',1),"
                               + "('Comol for Java. Version 2.1', 'javavm',2), "
                               + "('Comol for C. Version 2.0', 'custom',3), "
                               + "('Comol for C+. Version 2.1', 'native',4); ";
            InitTable(defCompil);

        }

        private void InitTable(string commands)
        {
            command.CommandText = commands;
            try
            {
                command.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                 ProntError(ex.Message);
            }
        }
        private bool IsNull()
        {
            DataTable allTableData = conn.GetSchema("Tables");
            if (allTableData.Rows.Count > 0)
            {
                return false;
            }
            else
            {
                StandCreateTables();
                return true;
            }
        }
        private bool ExistDataBase()
        {
            return (File.Exists(pathSQL));
        }

        private void ProntInfo(string mess)
        {
            MessageBox.Show(mess, "EvolPras", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ProntError(string mess)
        {
            MessageBox.Show(mess, "Exeptions", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
