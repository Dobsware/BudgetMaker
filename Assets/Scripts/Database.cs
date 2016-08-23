using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SQLiteDatabase;
using System.Collections.Generic;

public class Database : MonoBehaviour {

    #region Variables
    public static string[] Titles;                      //Each title of each Fase
    public static Image[,] OptionsImages;               //All the options images of each Fase
    public static string[,] OptionsTexts;               //All the options texts of each Fase
    public static int[,] OptionsHourCosts;             //All the options hours of each Fase
    public static float[,] OptionsPrices;               //All the options price of each Fase
    public static int NumberSets;

    public static int TotalNumberOfLists;
    public static int AppDesign;            //if 0 = não é agente a fazer o design da app, 1 = é agente a fazer o design da app
    public static int StoresDesign;         //if 0 = não é agente a por na loja / não é para por nas lojas, 1 = é agente a tratar das lojas
    #endregion

    public static int OrdenadosMultiplier;

    private int SetFaseInt;

    //.db plugin
//    SQLiteDB db = SQLiteDB.Instance;
//    List<string> allIDs = new List<string>();
//    List<string> allNames = new List<string>();
//    bool isTableCreated = false;

    #region USING DATABASE.CS

    public void DATABASEAwake ()    //Por isto em awake para funcionar
    {   
        #region Testing .db plugin example 

        /*
        db.DBLocation = Application.persistentDataPath;
        if (!db.Exists)
        {
            //CreateDB();
            db.ConnectToDefaultDatabase ("BudgetMaker.db",false);

            //Refresh()
            allIDs.Clear ();
            allNames.Clear ();

            // get all data from Users table
            DBReader reader = db.GetAllData("Answers");

            while(reader != null && reader.Read())
            {
                allIDs.Add(reader.GetStringValue("id"));
                allNames.Add(reader.GetStringValue("Question_id"));
                isTableCreated = true;
            }

            if (isTableCreated)
            {
                //Pre recorded values
//                for (int i = 0; i < allIDs.Count; i++)
//                {
//                    print("id: " + allIDs[i] + ", text: " + allNames[i]);
//                }

                //NORMAL QUERY
                print("QUERY");
                DBReader DBR = db.Select ("SELECT AnswerText FROM Answers WHERE Question_id = 3");

                foreach (Dictionary<string, object> dit in DBR.dataTable)
                    print(dit["AnswerText"]);


                //NO QUERY NEEDED
                SQLiteDB.DB_ConditionPair condition = new SQLiteDB.DB_ConditionPair ();
                condition.fieldName = "Question_id";
                condition.value = "12";
                condition.condition = SQLiteDB.DB_Condition.EQUAL_TO;

                List<string> fields = new List<string>();
                fields.Add("AnswerText");

                print("NOQUERY");
                DBReader DBR2 = db.Select("Answers", fields, condition);

                foreach (Dictionary<string, object> dit in DBR2.dataTable)
                    print(dit["AnswerText"]);
            }
            else
                print("Erro a ir buscar os dados da tabela");
        }
        else
            print("Não encontrei nenhuma pasta da db...");

        */

        #endregion

        SetFaseInt = 0;
        OrdenadosMultiplier = 2;

        #region Number of sets/fases + inits
        NumberSets = 26;
        Titles = new string[NumberSets];
        OptionsImages = new Image[NumberSets, 10];
        OptionsTexts = new string[NumberSets, 10];
        OptionsHourCosts = new int[NumberSets, 10];
        OptionsPrices = new float[NumberSets, 10];
        #endregion

        //Database
        #region 1st Fase (Design)

        Image[] TempImages = {Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image};

        string[] TempTexts = {"NADA", "Fazemos todo o Design da app + Lojas", "Apenas fazemos o Design da app", "Apenas fazemos o Design nas lojas"};
        int[] TempHours = {-1, -1, -1, -1};
        float[] TempPrices = {TempHours[0]*4.4f*OrdenadosMultiplier, TempHours[1]*4.4f*OrdenadosMultiplier, TempHours[2]*4.4f*OrdenadosMultiplier, TempHours[3]*4.4f*OrdenadosMultiplier}; 

        SetFase(SetFaseInt, "Design da App", TempImages, TempTexts, TempHours, TempPrices);
        SetFaseInt++;
        #endregion

        #region 2nd Fase (Plataformas = Tempo para Launch/Store/Icons/Banner)

        TempImages = new Image[] {Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image};

        TempTexts =  new string[] {"Android", "IOS", "Android + IOS", "Android + IOS + Windows Phone", "Web App", "Site", "Database + Back-offices para uma app", "TV", "Watch", "Outro..."};
        //Days Explanation:
        //(6)Android -> Icon:1 dia, Banner:2 dias, Screenshots:1 dia, Testing: 1 dia, Store put: 1 dia
        //(4)IOS -> Icon:1 dia, Screenshots:1 dia, Testing: 1dia, Store put: 1 dia
        //(7)Android + IOS -> Icon:1 dia, Banner:2 dias, Screenshots:1 dia, Testing: 1 dias, Store put: 2 dias
        //(9)Android + IOS + Windows Phone -> Icon:1 dia, Banner:2 dias, Screenshots:1 dia, Testing: 2 dias, Store put: 3 dias
        //(1)Web App -> Testing: 1 dia
        //(1)Site -> Wordpress create: 1 dia... (falta por vez o resto...)
        //(1 + 0.5*Listas)Database + Back-offices -> Database: 1 dia + 5dias (Max of Listas = 10)
        //(10)Android + IOS TV -> Icon:1 dia, Banner:2 dias, Screenshots:1 dia, Testing: 1 dias, Store put: 2 dias, Para aprender: 3 dias
        //(10)Android + IOS Watch -> Icon:1 dia, Banner:2 dias, Screenshots:1 dia, Testing: 1 dias, Store put: 2 dias, Para aprender: 3 dias
        TempHours =  new int[] {5*8*StoresDesign + 1*8, 3*8*StoresDesign + 1*8, 6*8*StoresDesign + 1*8, 7*8*StoresDesign + 2*8, 1*8, 1*8, 6*8, 6*8*StoresDesign + 4*8, 6*8*StoresDesign + 4*8, -1};
        TempPrices = new float[] {TempHours[0]*4.4f*OrdenadosMultiplier, TempHours[1]*4.4f*OrdenadosMultiplier, TempHours[2]*4.4f*OrdenadosMultiplier, TempHours[3]*4.4f*OrdenadosMultiplier, TempHours[4]*4.4f*OrdenadosMultiplier, TempHours[5]*4.4f*OrdenadosMultiplier,
                            TempHours[6]*4.4f*OrdenadosMultiplier, TempHours[7]*4.4f*OrdenadosMultiplier, TempHours[8]*4.4f*OrdenadosMultiplier, TempHours[9]*4.4f*OrdenadosMultiplier}; 
        
        SetFase(SetFaseInt, "Plataformas", TempImages, TempTexts, TempHours, TempPrices);
        SetFaseInt++;
        #endregion

        #region 3rd Fase (Main Menu)

        TempImages = new Image[] {Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image};

        TempTexts =  new string[] {"NADA", "Sim"};
        //(2.5)Main Menu -> 2dias art + 0.5program
        TempHours =  new int[] {0, 2*8*AppDesign + 4};
        TempPrices = new float[] {TempHours[0]*4.4f*OrdenadosMultiplier, TempHours[1]*4.4f*OrdenadosMultiplier}; 

        SetFase(SetFaseInt, "Main Menu", TempImages, TempTexts, TempHours, TempPrices);
        SetFaseInt++;
        #endregion

        #region 4th Fase (Tempo para cada scene diferente)

        TempImages = new Image[] {Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image};

        TempTexts = new string[] {"NADA", "1", "2", "3", "4", "5", "6", "7", "8", "9 ou mais"};
        //(4) Cada Menu -> 2dias Art, 2 dias Program
        TempHours = new int[] {0, 1*2*8*AppDesign + 1*2*8, 2*2*8*AppDesign + 2*2*8, 3*2*8*AppDesign + 3*2*8, 4*2*8*AppDesign + 4*2*8, 5*2*8*AppDesign + 5*2*8,
                                    6*2*8*AppDesign + 6*2*8, 7*2*8*AppDesign + 7*2*8, 8*2*8*AppDesign + 8*2*8, 9*2*8*AppDesign + 9*2*8};
        TempPrices = new float[] {TempHours[0]*4.4f*OrdenadosMultiplier, TempHours[1]*4.4f*OrdenadosMultiplier, TempHours[2]*4.4f*OrdenadosMultiplier, TempHours[3]*4.4f*OrdenadosMultiplier, TempHours[4]*4.4f*OrdenadosMultiplier, TempHours[5]*4.4f*OrdenadosMultiplier, TempHours[6]*4.4f*OrdenadosMultiplier, TempHours[7]*4.4f*OrdenadosMultiplier, TempHours[8]*4.4f*OrdenadosMultiplier, TempHours[9]*4.4f*OrdenadosMultiplier}; 

        SetFase(SetFaseInt, "Número de Menus diferentes\n(Excepto Main Menu)", TempImages, TempTexts, TempHours, TempPrices);
        SetFaseInt++;
        #endregion

        #region 5th Fase (Tempo de importar os dados para a app)

        TempImages = new Image[] {Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image};

        TempTexts = new string[] {"NADA", "Excel feito pelo cliente", "Excel feito por nós", "Base de dados já criada", "Base de dados a ser criada"};
        //(1dia x Listas) Excel feito pelo cliente -> Debug Excel: 1 dia Program (x Listas)
        //(1dia + 0.5dias x Listas) Base de dados a ser criada -> 1dia Database create, JSON Create to Database or vice-versa: 1/2dia (x Listas)
        TempHours = new int[] {0, TotalNumberOfLists*8, -1, 0, 1*8 + 1*4*TotalNumberOfLists};
        TempPrices = new float[] {TempHours[0]*4.4f*OrdenadosMultiplier, TempHours[1]*4.4f*OrdenadosMultiplier, TempHours[2]*4.4f*OrdenadosMultiplier, TempHours[3]*4.4f*OrdenadosMultiplier, TempHours[4]*4.4f*OrdenadosMultiplier}; 

        SetFase(SetFaseInt, "Como importar os dados para a app", TempImages, TempTexts, TempHours, TempPrices);
        SetFaseInt++;
        #endregion

        #region 6th Fase (Se introdução dos dados será feito por nós -> tempo por cada 50 produtos/pontos)

        TempImages = new Image[] {Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image};

        TempTexts = new string[] {"É necessário Back-Offices", "50", "100", "150", "200", "300", "400", "600", "800", "1000 ou mais"};
        //(1) Cada 50 linhas = 1dia program
        TempHours = new int[] {-1, 1*8, 2*8, 3*8, 4*8, 6*8, 8*8, 12*8, 16*8, 20*8};
        TempPrices = new float[] {TempHours[0]*4.4f*OrdenadosMultiplier, TempHours[1]*4.4f*OrdenadosMultiplier, TempHours[2]*4.4f*OrdenadosMultiplier, TempHours[3]*4.4f*OrdenadosMultiplier, TempHours[4]*4.4f*OrdenadosMultiplier, TempHours[5]*4.4f*OrdenadosMultiplier, TempHours[6]*4.4f*OrdenadosMultiplier, TempHours[7]*4.4f*OrdenadosMultiplier, TempHours[8]*4.4f*OrdenadosMultiplier, TempHours[9]*4.4f*OrdenadosMultiplier}; 

        SetFase(SetFaseInt, "Total de Produtos/Pontos/etc importados por nós", TempImages, TempTexts, TempHours, TempPrices);
        SetFaseInt++;
        #endregion

        #region 7th Fase (Se introdução dos dados será feito pelo cliente & Base de dados -> tempo de back-offices)

        TempImages = new Image[] {Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image};

        TempTexts = new string[] {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9 ou mais"};
        //(4) Cada back-office -> 4 dias program
        TempHours = new int[] {0, 4*8, 8*8, 12*8, 16*8, 20*8, 24*8, 28*8, 32*8, 36*8};
        TempPrices = new float[] {TempHours[0]*4.4f*OrdenadosMultiplier, TempHours[1]*4.4f*OrdenadosMultiplier, TempHours[2]*4.4f*OrdenadosMultiplier, TempHours[3]*4.4f*OrdenadosMultiplier, TempHours[4]*4.4f*OrdenadosMultiplier, TempHours[5]*4.4f*OrdenadosMultiplier, TempHours[6]*4.4f*OrdenadosMultiplier, TempHours[7]*4.4f*OrdenadosMultiplier, TempHours[8]*4.4f*OrdenadosMultiplier, TempHours[9]*4.4f*OrdenadosMultiplier}; 

        SetFase(SetFaseInt, "Total de Back-offices para editar produtos/pontos/etc", TempImages, TempTexts, TempHours, TempPrices);
        SetFaseInt++;
        #endregion

        #region 8th Fase (Cada aditional view de back-offices)

        TempImages = new Image[] {Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image};

        TempTexts = new string[] {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9 ou mais"};
        //(1) Cada back-office View -> 1 dia program
        TempHours = new int[] {0, 1*8, 2*8, 3*8, 4*8, 5*8, 6*8, 7*8, 8*8, 9*8};
        TempPrices = new float[] {TempHours[0]*4.4f*OrdenadosMultiplier, TempHours[1]*4.4f*OrdenadosMultiplier, TempHours[2]*4.4f*OrdenadosMultiplier, TempHours[3]*4.4f*OrdenadosMultiplier, TempHours[4]*4.4f*OrdenadosMultiplier, TempHours[5]*4.4f*OrdenadosMultiplier, TempHours[6]*4.4f*OrdenadosMultiplier, TempHours[7]*4.4f*OrdenadosMultiplier, TempHours[8]*4.4f*OrdenadosMultiplier, TempHours[9]*4.4f*OrdenadosMultiplier}; 

        SetFase(SetFaseInt, "Total de Back-offices para Visualização de dados apenas", TempImages, TempTexts, TempHours, TempPrices);
        SetFaseInt++;
        #endregion

        #region 9th Fase (Geo)

        TempImages = new Image[] {Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image};

        TempTexts = new string[] {"NADA", "Online apenas", "Offline apenas com mapa das estradas", "Offline apenas com mapa de satelite", "Offline (estradas) e Online", "Offline (satelite) e Online"};
        TempHours = new int[] {0, 1*4, 1*8, 2*8, 1*8 + 1*4, 2*8 + 1*4};
        TempPrices = new float[] {TempHours[0]*4.4f*OrdenadosMultiplier, TempHours[1]*4.4f*OrdenadosMultiplier, TempHours[2]*4.4f*OrdenadosMultiplier, TempHours[3]*4.4f*OrdenadosMultiplier, TempHours[4]*4.4f*OrdenadosMultiplier, TempHours[5]*4.4f*OrdenadosMultiplier}; 

        SetFase(SetFaseInt, "Georreferenciação?", TempImages, TempTexts, TempHours, TempPrices);
        SetFaseInt++;
        #endregion

        #region 10th Fase (Geo)

        TempImages = new Image[] {Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image};

        TempTexts = new string[] {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9 ou mais"};
        //(5horas) Cada Icon -> 1 horas Art + 4horas Program
        TempHours = new int[] {0, 1*5*AppDesign, 2*5*AppDesign, 3*5*AppDesign, 4*5*AppDesign, 5*5*AppDesign, 6*5*AppDesign, 7*5*AppDesign, 8*5*AppDesign, 9*5*AppDesign};
        TempPrices = new float[] {TempHours[0]*4.4f*OrdenadosMultiplier, TempHours[1]*4.4f*OrdenadosMultiplier, TempHours[2]*4.4f*OrdenadosMultiplier, TempHours[3]*4.4f*OrdenadosMultiplier, TempHours[4]*4.4f*OrdenadosMultiplier, TempHours[5]*4.4f*OrdenadosMultiplier, TempHours[6]*4.4f*OrdenadosMultiplier, TempHours[7]*4.4f*OrdenadosMultiplier, TempHours[8]*4.4f*OrdenadosMultiplier, TempHours[9]*4.4f*OrdenadosMultiplier}; 

        SetFase(SetFaseInt, "Número de tipos de pontos diferentes de georreferenciação?", TempImages, TempTexts, TempHours, TempPrices);
        SetFaseInt++;
        #endregion

        #region 11th Fase (QR Codes)

        TempImages = new Image[] {Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image};

        TempTexts = new string[] {"NADA", "Apenas para as Stores", "10", "20", "30", "40", "50", "70", "90", "Mais de 100"};
        TempHours = new int[] {0, 1*4, 1*12 + 10*2, 1*12 + 20*2, 1*12 + 30*2, 1*12 + 40*2, 1*12 + 50*2, 1*12 + 70*2, 1*12 + 90*2, 1*12 + 100*2};
        TempPrices = new float[] {TempHours[0]*4.4f*OrdenadosMultiplier, TempHours[1]*4.4f*OrdenadosMultiplier, TempHours[2]*4.4f*OrdenadosMultiplier, TempHours[3]*4.4f*OrdenadosMultiplier, TempHours[4]*4.4f*OrdenadosMultiplier, TempHours[5]*4.4f*OrdenadosMultiplier, TempHours[6]*4.4f*OrdenadosMultiplier, TempHours[7]*4.4f*OrdenadosMultiplier, TempHours[8]*4.4f*OrdenadosMultiplier, TempHours[9]*4.4f*OrdenadosMultiplier}; 

        SetFase(SetFaseInt, "Número de QR Codes", TempImages, TempTexts, TempHours, TempPrices);
        SetFaseInt++;
        #endregion

        #region 12th Fase (In-Apps)

        TempImages = new Image[] {Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image};

        TempTexts = new string[] {"NADA", "Sim"};
        //(2) In-Apps = Program+Store = 1dia, Buttons = 1dia
        TempHours = new int[] {0, 1*8 + 1*8*AppDesign};
        TempPrices = new float[] {TempHours[0]*4.4f*OrdenadosMultiplier, TempHours[1]*4.4f*OrdenadosMultiplier}; 

        SetFase(SetFaseInt, "Compras na aplicação?", TempImages, TempTexts, TempHours, TempPrices);
        SetFaseInt++;
        #endregion

        #region 13th Fase (AdMob)

        TempImages = new Image[] {Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image};

        TempTexts = new string[] {"NADA", "Sim"};
        TempHours = new int[] {0, 1*8};
        TempPrices = new float[] {TempHours[0]*4.4f*OrdenadosMultiplier, TempHours[1]*4.4f*OrdenadosMultiplier}; 

        SetFase(SetFaseInt, "Publicidade AdMob?", TempImages, TempTexts, TempHours, TempPrices);
        SetFaseInt++;
        #endregion

        #region 14th Fase (Linguas)

        TempImages = new Image[] {Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image};

        TempTexts = new string[] {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10"};
        //Days Explanation:
        //(1.5 + Listas/3) Cada Linguas adicional á primeira: 
        //      Preparar para o cliente: 1/2 (xLinguas) 
        //      Program Botao+Funcionando: 1/2 (xLinguas)
        //      Lojas: 1/2 (xLinguas)
        //      Traduzir cada 3 Menus (titulos/etc...): 1/2 (xLinguas)
        TempHours = new int[] {0, 8 + 1*4*StoresDesign + (int)(TotalNumberOfLists/3), 8 + 2*4*StoresDesign + (int)(TotalNumberOfLists/3), 8 + 3*4*StoresDesign + (int)(TotalNumberOfLists/3), 8 + 4*4*StoresDesign + (int)(TotalNumberOfLists/3), 8 + 5*4*StoresDesign + (int)(TotalNumberOfLists/3),
                                    8 + 6*4*StoresDesign + (int)(TotalNumberOfLists/3), 8 + 7*4*StoresDesign + (int)(TotalNumberOfLists/3), 8 + 8*4*StoresDesign + (int)(TotalNumberOfLists/3), 8 + 9*4*StoresDesign + (int)(TotalNumberOfLists/3)};
        TempPrices = new float[] {TempHours[0]*4.4f*OrdenadosMultiplier, TempHours[1]*4.4f*OrdenadosMultiplier, TempHours[2]*4.4f*OrdenadosMultiplier, TempHours[3]*4.4f*OrdenadosMultiplier, TempHours[4]*4.4f*OrdenadosMultiplier, TempHours[5]*4.4f*OrdenadosMultiplier, TempHours[6]*4.4f*OrdenadosMultiplier, TempHours[7]*4.4f*OrdenadosMultiplier, TempHours[8]*4.4f*OrdenadosMultiplier, TempHours[9]*4.4f*OrdenadosMultiplier}; 

        SetFase(SetFaseInt, "Número de Linguas", TempImages, TempTexts, TempHours, TempPrices);
        SetFaseInt++;
        #endregion

        #region 15th Fase (Contactos)

        TempImages = new Image[] {Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image};

        TempTexts = new string[] {"NADA", "Sim"};
        //(1) Contactos = 1/2 dia Art, 1/2 dia program
        TempHours = new int[] {0, 1*4*AppDesign + 1*4};
        TempPrices = new float[] {TempHours[0]*4.4f*OrdenadosMultiplier, TempHours[1]*4.4f*OrdenadosMultiplier}; 

        SetFase(SetFaseInt, "Contactos?\n(Email / Telefone / WebSite)", TempImages, TempTexts, TempHours, TempPrices);
        SetFaseInt++;
        #endregion

        #region 16th Fase (Favoritos)

        TempImages = new Image[] {Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image};

        TempTexts = new string[] {"NADA", "Sim"};
        TempHours = new int[] {0, 1*8};
        TempPrices = new float[] {TempHours[0]*4.4f*OrdenadosMultiplier, TempHours[1]*4.4f*OrdenadosMultiplier}; 

        SetFase(SetFaseInt, "Favoritos?", TempImages, TempTexts, TempHours, TempPrices);
        SetFaseInt++;
        #endregion

        #region 17th Fase (Historico)

        TempImages = new Image[] {Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image};

        TempTexts = new string[] {"NADA", "Sim"};
        TempHours = new int[] {0, 1*8};
        TempPrices = new float[] {TempHours[0]*4.4f*OrdenadosMultiplier, TempHours[1]*4.4f*OrdenadosMultiplier}; 

        SetFase(SetFaseInt, "Historico?", TempImages, TempTexts, TempHours, TempPrices);
        SetFaseInt++;
        #endregion

        #region 18th Fase (Share)

        TempImages = new Image[] {Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image};

        TempTexts = new string[] {"NADA", "Sim"};
        TempHours = new int[] {0, 1*4};
        TempPrices = new float[] {TempHours[0]*4.4f*OrdenadosMultiplier, TempHours[1]*4.4f*OrdenadosMultiplier}; 

        SetFase(SetFaseInt, "Share Global?", TempImages, TempTexts, TempHours, TempPrices);
        SetFaseInt++;
        #endregion

        #region 19th Fase (Login)

        TempImages = new Image[] {Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image};

        TempTexts = new string[] {"NADA", "Facebook", "Google Play ou Game Center", "Personalizado numa base de dados", "Vários"};
        TempHours = new int[] {0, 1*8, 1*8, 1*8, 2*8};
        TempPrices = new float[] {TempHours[0]*4.4f*OrdenadosMultiplier, TempHours[1]*4.4f*OrdenadosMultiplier, TempHours[2]*4.4f*OrdenadosMultiplier, TempHours[3]*4.4f*OrdenadosMultiplier, TempHours[4]*4.4f*OrdenadosMultiplier}; 

        SetFase(SetFaseInt, "Algum tipo de login?", TempImages, TempTexts, TempHours, TempPrices);
        SetFaseInt++;
        #endregion

        #region 20th Fase (Facebook friends)

        TempImages = new Image[] {Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image};

        TempTexts = new string[] {"NADA", "Sim"};
        TempHours = new int[] {0, 2*8};
        TempPrices = new float[] {TempHours[0]*4.4f*OrdenadosMultiplier, TempHours[1]*4.4f*OrdenadosMultiplier}; 

        SetFase(SetFaseInt, "Requer informação dos amigos do Facebook?", TempImages, TempTexts, TempHours, TempPrices);
        SetFaseInt++;
        #endregion

        #region 21st Fase (Sistema de comentarios)

        TempImages = new Image[] {Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image};

        TempTexts = new string[] {"NADA", "Sim"};
        //(3) Comentatios -> program: 3 dias
        TempHours = new int[] {0, 3*8};
        TempPrices = new float[] {TempHours[0]*4.4f*OrdenadosMultiplier, TempHours[1]*4.4f*OrdenadosMultiplier}; 

        SetFase(SetFaseInt, "Sistema de Comentarios?", TempImages, TempTexts, TempHours, TempPrices);
        SetFaseInt++;
        #endregion

        #region 22nd Fase (Analytics)

        TempImages = new Image[] {Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image};

        TempTexts = new string[] {"NADA", "Sim"};
        TempHours = new int[] {0, 2*8};
        TempPrices = new float[] {TempHours[0]*4.4f*OrdenadosMultiplier, TempHours[1]*4.4f*OrdenadosMultiplier}; 

        SetFase(SetFaseInt, "Analytics?", TempImages, TempTexts, TempHours, TempPrices);
        SetFaseInt++;
        #endregion

        #region 23rd Fase (Reuniões)

        TempImages = new Image[] {Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image};

        TempTexts = new string[] {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10"};
        //(1/2) Cada Reuniao: 1/2 dia
        TempHours = new int[] {1*4, 2*4, 3*4, 4*4, 5*4, 6*4, 7*4, 8*4, 9*4, 10*4};
        TempPrices = new float[] {TempHours[0]*4.4f*OrdenadosMultiplier, TempHours[1]*4.4f*OrdenadosMultiplier, TempHours[2]*4.4f*OrdenadosMultiplier, TempHours[3]*4.4f*OrdenadosMultiplier, TempHours[4]*4.4f*OrdenadosMultiplier, TempHours[5]*4.4f*OrdenadosMultiplier, TempHours[6]*4.4f*OrdenadosMultiplier, TempHours[7]*4.4f*OrdenadosMultiplier, TempHours[8]*4.4f*OrdenadosMultiplier, TempHours[9]*4.4f*OrdenadosMultiplier}; 

        SetFase(SetFaseInt, "Número de Fases/Reuniões", TempImages, TempTexts, TempHours, TempPrices);
        SetFaseInt++;
        #endregion

        #region 24th Fase (Gamify)

        TempImages = new Image[] {Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image};

        TempTexts = new string[] {"Free", "Free?", "Free!", "Really Free?!", "Yes Free!?"};
        TempHours = new int[] {0, 0, 0, 0, 0};
        TempPrices = new float[] {TempHours[0]*4.4f*OrdenadosMultiplier, TempHours[1]*4.4f*OrdenadosMultiplier, TempHours[2]*4.4f*OrdenadosMultiplier, TempHours[3]*4.4f*OrdenadosMultiplier, TempHours[4]*4.4f*OrdenadosMultiplier}; 

        SetFase(SetFaseInt, "Gamify?", TempImages, TempTexts, TempHours, TempPrices);
        SetFaseInt++;
        #endregion

        #region 25th Fase (Outros Dias Art)

        TempImages = new Image[] {Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image};

        TempTexts = new string[] {"0", "1", "2", "3", "4", "5", "7", "10", "15", "20 ou mais"};
        TempHours = new int[] {0, 1*8, 2*8, 3*8, 4*8, 5*8, 7*8, 10*8, 15*8, 20*8};
        TempPrices = new float[] {TempHours[0]*4.4f*OrdenadosMultiplier, TempHours[1]*4.4f*OrdenadosMultiplier, TempHours[2]*4.4f*OrdenadosMultiplier, TempHours[3]*4.4f*OrdenadosMultiplier, TempHours[4]*4.4f*OrdenadosMultiplier
                                    ,TempHours[5]*4.4f*OrdenadosMultiplier, TempHours[6]*4.4f*OrdenadosMultiplier, TempHours[7]*4.4f*OrdenadosMultiplier, TempHours[8]*4.4f*OrdenadosMultiplier, TempHours[9]*4.4f*OrdenadosMultiplier}; 

        SetFase(SetFaseInt, "Quantos dias de art?", TempImages, TempTexts, TempHours, TempPrices);
        SetFaseInt++;
        #endregion

        #region 26th Fase (Outros Dias Program)

        TempImages = new Image[] {Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image};

        TempTexts = new string[] {"0", "1", "2", "3", "4", "5", "7", "10", "15", "20 ou mais"};
        TempHours = new int[] {0, 1*8, 2*8, 3*8, 4*8, 5*8, 7*8, 10*8, 15*8, 20*8};
        TempPrices = new float[] {TempHours[0]*4.4f*OrdenadosMultiplier, TempHours[1]*4.4f*OrdenadosMultiplier, TempHours[2]*4.4f*OrdenadosMultiplier, TempHours[3]*4.4f*OrdenadosMultiplier, TempHours[4]*4.4f*OrdenadosMultiplier
                                    ,TempHours[5]*4.4f*OrdenadosMultiplier, TempHours[6]*4.4f*OrdenadosMultiplier, TempHours[7]*4.4f*OrdenadosMultiplier, TempHours[8]*4.4f*OrdenadosMultiplier, TempHours[9]*4.4f*OrdenadosMultiplier};
        
        SetFase(SetFaseInt, "Quantos dias de program?", TempImages, TempTexts, TempHours, TempPrices);
        SetFaseInt++;
        #endregion

        /*
        #region 28th Fase (Site)   //Pagamentos(5), Numero Pontos/Conteudo (100 por dia), Login (5), Agente a fazer o excel

        TempImages = new Image[] {Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image,
            Resources.Load("Options Images/Panel") as Image};

        TempTexts = new string[] {"0", "1", "2", "3", "4", "5", "7", "10", "15", "20 ou mais"};
        TempHours = new int[] {0, 1*8, 2*8, 3*8, 4*8, 5*8, 7*8, 10*8, 15*8, 20*8};
        TempPrices = new float[] {TempHours[0]*4.4f*OrdenadosMultiplier, TempHours[1]*4.4f*OrdenadosMultiplier, TempHours[2]*4.4f*OrdenadosMultiplier, TempHours[3]*4.4f*OrdenadosMultiplier, TempHours[4]*4.4f*OrdenadosMultiplier}; 

        SetFase(SetFaseInt, "Quantos dias?", TempImages, TempTexts, TempHours, TempPrices);
        SetFaseInt++;
        #endregion
        */
    }

    #region SetFase
    void SetFase(int i, string title, Image[] images, string[] texts, int[] hours, float[] prices)
    {
        Titles[i] = title;

        for (int a = 0; a < images.Length; a++)
            OptionsImages[i, a] = images[a];
        for (int a = 0; a < texts.Length; a++)
            OptionsTexts[i, a] = texts[a];
        for (int a = 0; a < hours.Length; a++)
            OptionsHourCosts[i, a] = hours[a];
        for (int a = 0; a < prices.Length; a++)
            OptionsPrices[i, a] = prices[a];
    }
    #endregion

    #endregion
}
