using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;
using System.Collections.Generic;
using SQLiteDatabase;

public class Controller : MonoBehaviour {

    #region Variables

    //private int CurrentFase;
    private float CurrentMoney;
    private int CurrentTime;
    private int CurrentUnityTime;
    private int CurrentWordPressTime;
    private int CurrentSQLTime;
    private int CurrentArtTime;
    private int CurrentStoreTime;
    private int CurrentOutrosTime;

    private Stack BackFaseStack = new Stack();
    private Stack BackTimeStack = new Stack();
    private Stack BackUnityTimeStack = new Stack();
    private Stack BackWordPressTimeStack = new Stack();
    private Stack BackSQLTimeStack = new Stack();
    private Stack BackArtTimeStack = new Stack();
    private Stack BackStoreTimeStack = new Stack();
    private Stack BackOutrosTimeStack = new Stack();
    private Stack BackMoneyStack = new Stack();

    public static Image[] OptionsImages;           //All the options images SELECTED of each Fase
    public static string[] OptionsTexts;           //All the options texts SELECTED of each Fase
    public static float[] OptionsPrice;           //All the options price SELECTED of each Fase
    public static float[] OptionsUnityTime;           //All the options time SELECTED of each Fase
    public static float[] OptionsWordPressTime;           //All the options time SELECTED of each Fase
    public static float[] OptionsSQLTime;           //All the options time SELECTED of each Fase
    public static float[] OptionsArtTime;           //All the options time SELECTED of each Fase
    public static float[] OptionsStoreTime;           //All the options time SELECTED of each Fase
    public static float[] OptionsOutrosTime;           //All the options time SELECTED of each Fase

    public GameObject ChoicePrefab;
    public Text Title;
    public GameObject Inputfield;
    public Canvas TheCanvas;
    public Text Results;
    public Text Current;
    public Text FaturaText;

    //.db plugin
    SQLiteDB db = SQLiteDB.Instance;
    DBReader QuestionsTable;
    int CurrentQuestionID;
    public Text Caminho;

    #endregion

    #region Start

	void Start () 
    {
        //USING BUDGETMAKER.DB
        db.DBLocation = Application.persistentDataPath;
        Caminho.text = Application.persistentDataPath;

        if (db.Exists) {
            db.ConnectToDefaultDatabase ("BudgetMaker2.db",false);
        } else {
            db.ConnectToDefaultDatabase ("BudgetMaker2.db",true);
        }

        QuestionsTable = db.GetAllData("Questions");
        CurrentQuestionID = 0;


        //CurrentFase = 0;
        CurrentTime = 0;
        CurrentMoney = CurrentTime*4.4f*Database.OrdenadosMultiplier;
        OptionsImages = new Image[QuestionsTable.dataTable.Count + 1];
        OptionsTexts = new string[QuestionsTable.dataTable.Count + 1];
        OptionsPrice = new float[QuestionsTable.dataTable.Count + 1];
        OptionsUnityTime = new float[QuestionsTable.dataTable.Count + 1];
        OptionsWordPressTime = new float[QuestionsTable.dataTable.Count + 1];
        OptionsSQLTime = new float[QuestionsTable.dataTable.Count + 1];
        OptionsArtTime = new float[QuestionsTable.dataTable.Count + 1];
        OptionsStoreTime = new float[QuestionsTable.dataTable.Count + 1];
        OptionsOutrosTime = new float[QuestionsTable.dataTable.Count + 1];
       
        //Build First Fase
        DoFase();
	}

    #endregion
	
    #region DoNextFase

    public void DoNextFase (int thechoise) 
    {
        #region USING DATABASE.CS
        //RecordThisFase
//        if(Database.OptionsHourCosts[CurrentFase, thechoise] != -1f)     //-1 = 0 in fact!
//            CurrentTime += Database.OptionsHourCosts[CurrentFase, thechoise];
//
//        if(Database.OptionsPrices[CurrentFase, thechoise] != -8.8f)     //-8.8 = 0 in fact!
//            CurrentMoney += Database.OptionsPrices[CurrentFase, thechoise];
//
//        OptionsImages[CurrentFase] = Database.OptionsImages[CurrentFase, thechoise];
//        OptionsTexts[CurrentFase] = Database.OptionsTexts[CurrentFase, thechoise];
//        OptionsPrice[CurrentFase] = Database.OptionsPrices[CurrentFase, thechoise];

        //Destroy AllPrevious Options
//        GameObject[] options = GameObject.FindGameObjectsWithTag("Options");
//        foreach (GameObject option in options)
//            Destroy(option);

        //Record Back Fase
        //        BackFaseStack.Push(CurrentFase);
//
//        if(Database.OptionsHourCosts[CurrentFase, thechoise] != -1f)     //-1 = 0 in fact!
//            BackTimeStack.Push(Database.OptionsHourCosts[CurrentFase, thechoise]);
//        else
//            BackTimeStack.Push(0);
//
//        if(Database.OptionsPrices[CurrentFase, thechoise] != -8.8f)     //-8.8 = 0 in fact!
//            BackMoneyStack.Push(Database.OptionsPrices[CurrentFase, thechoise]);
//        else
//            BackMoneyStack.Push(0.0f);
        #endregion

        //USING BUDGETMAKER.DB
        SelectFase(thechoise);
        DBReader query = db.Select ("SELECT Next_Question_id, AnswerText, Horas_Unity, Horas_WordPress, Horas_SQL, Horas_Art, Horas_Store, Horas_Outros, Price FROM Answers WHERE Question_id = " + CurrentQuestionID);

        int thistime = 0;
        if (query.dataTable[thechoise]["Horas_Unity"].ToString() != "-1")     //-1 = 0 in fact!
        {
            //Busca os tempos da escolha
            int ThisUnityTime = int.Parse(query.dataTable[thechoise]["Horas_Unity"].ToString());
            int ThisWordPressTime = int.Parse(query.dataTable[thechoise]["Horas_WordPress"].ToString());
            int ThisSQLTime = int.Parse(query.dataTable[thechoise]["Horas_SQL"].ToString());
            int ThisArtTime = int.Parse(query.dataTable[thechoise]["Horas_Art"].ToString())*Database.AppDesign;
            int ThisStoreTime = int.Parse(query.dataTable[thechoise]["Horas_Store"].ToString())*Database.StoresDesign;
            int ThisOutrosTime = int.Parse(query.dataTable[thechoise]["Horas_Outros"].ToString());

            thistime = ThisUnityTime + ThisWordPressTime + ThisSQLTime + ThisArtTime + ThisStoreTime + ThisOutrosTime;

            //soma aos tempos totais
            CurrentTime += thistime;
            CurrentUnityTime += ThisUnityTime;
            CurrentWordPressTime += ThisWordPressTime;
            CurrentSQLTime += ThisSQLTime;
            CurrentArtTime += ThisArtTime;
            CurrentStoreTime += ThisStoreTime;
            CurrentOutrosTime += ThisOutrosTime;

            //grava tudo
            //OptionsImages[CurrentQuestionID] = Database.OptionsImages[CurrentFase, thechoise];  //falta imagem
            OptionsTexts[CurrentQuestionID] = query.dataTable[thechoise]["AnswerText"].ToString();

            OptionsUnityTime[CurrentQuestionID] = ThisUnityTime;
            OptionsWordPressTime[CurrentQuestionID] = ThisWordPressTime;
            OptionsSQLTime[CurrentQuestionID] = ThisSQLTime;
            OptionsArtTime[CurrentQuestionID] = ThisArtTime;
            OptionsStoreTime[CurrentQuestionID] = ThisStoreTime;
            OptionsOutrosTime[CurrentQuestionID] = ThisOutrosTime;

            if (query.dataTable[thechoise]["Price"].ToString() == "")
                OptionsPrice[CurrentQuestionID] = thistime * Database.OrdenadosMultiplier * 4.4f;
            else
                OptionsPrice[CurrentQuestionID] = int.Parse(query.dataTable[thechoise]["Price"].ToString()); 

            CurrentMoney += OptionsPrice[CurrentQuestionID];

            //grava os tempos/preço para 'back'
            BackTimeStack.Push(thistime);
            BackUnityTimeStack.Push(ThisUnityTime);
            BackWordPressTimeStack.Push(ThisWordPressTime);
            BackSQLTimeStack.Push(ThisSQLTime);
            BackArtTimeStack.Push(ThisArtTime);
            BackStoreTimeStack.Push(ThisStoreTime);
            BackOutrosTimeStack.Push(ThisOutrosTime);
            BackMoneyStack.Push(OptionsPrice[CurrentQuestionID]);
        }
        else  //caso resposta sem valor de horas
        {
            //grava apenas os tempos/preço para 'back'
            BackTimeStack.Push(0);
            BackUnityTimeStack.Push(0);
            BackWordPressTimeStack.Push(0);
            BackSQLTimeStack.Push(0);
            BackArtTimeStack.Push(0);
            BackStoreTimeStack.Push(0);
            BackOutrosTimeStack.Push(0);
            BackMoneyStack.Push(0.0f);
        }

        //Destroy AllPrevious Options
        GameObject[] options = GameObject.FindGameObjectsWithTag("Options");
        foreach (GameObject option in options)
            Destroy(option);

        //Record Back Fase
        BackFaseStack.Push(CurrentQuestionID);

        //Build Next Fase
        CurrentQuestionID = int.Parse(query.dataTable[thechoise]["Next_Question_id"].ToString());

        if (CurrentQuestionID <= QuestionsTable.dataTable.Count)      //ERROR AQUI MAYBE!
            DoFase();
        else
            DoTheEnd();
	}

    #endregion

    #region SelectFase 

    public void SelectFase(int thechoise)
    {
        #region USING DATABASE.CS

//        //print("Fase: " + CurrentFase + ". Choice: " + thechoise);
//        if (Database.Titles[CurrentFase] == "Design da App")
//        {
//            if (thechoise == 0)      //NADA
//            {
//                Database.AppDesign = 0;
//                Database.StoresDesign = 0;
//            }
//            else if (thechoise == 1)      //We do everything
//            {
//                Database.AppDesign = 1;
//                Database.StoresDesign = 1;
//            }
//            else if (thechoise == 2)      //We do only the app design
//            {
//                Database.AppDesign = 1;
//                Database.StoresDesign = 0;
//            }
//            else if (thechoise == 3)      //We do only the stores design
//            {
//                Database.AppDesign = 0;
//                Database.StoresDesign = 1;
//            }
//
//            CurrentFase += 1;
//            GameObject.Find("Database").GetComponent<Database>().Awake();
//        }
//        else if (Database.Titles[CurrentFase] == "Plataformas")   
//        {
//            if (thechoise == 6)         //Database + Back-offices
//                CurrentFase += 5;
//            else if (thechoise == 5)      //Site
//                CurrentFase += 23;
//            else if (thechoise == 9)      //Outro
//                CurrentFase += 23;
//            else
//                CurrentFase += 1;
//        }
//        else if (Database.Titles[CurrentFase] == "Número de Menus diferentes\n(Excepto Main Menu)")   
//        {
//            if (thechoise == 0)      //Zero Listas
//                CurrentFase += 5;
//            else
//                CurrentFase += 1;
//
//            Database.TotalNumberOfLists = thechoise;
//            GameObject.Find("Database").GetComponent<Database>().Awake();
//        }
//        else if (Database.Titles[CurrentFase] == "Como importar os dados para a app")   
//        {
//            if (thechoise == 2 || thechoise == 4)      //excel feito por nós ou New Base de dados 
//                CurrentFase += 1;
//            else
//                CurrentFase += 4;
//        }
//        else if (Database.Titles[CurrentFase] == "Total de Produtos/Pontos/etc importados por nós")   
//        {
//            if (thechoise == 0)      //Need Back-offices
//                CurrentFase += 1;
//            else
//                CurrentFase += 3;   //Dont need back-offices
//        }
//        else if (Database.Titles[CurrentFase] == "Número de tipos de pontos diferentes de georreferenciação?")
//        {
//            if (thechoise == 0)      //Não é necessário geolocalização
//                CurrentFase += 2;
//            else
//                CurrentFase += 1;
//        }
//        else if (Database.Titles[CurrentFase] == "Algum tipo de login?")   
//        {
//            if (thechoise == 1 || thechoise == 4)      //Usar facebook
//                CurrentFase += 1;
//            else
//                CurrentFase += 2;
//        }
//        else if (Database.Titles[CurrentFase] == "Gamify?")   
//        {
//            CurrentFase += 3;
//        }
//        else
//        {
//            CurrentFase += 1;
//        }

        #endregion

        //USING BUDGETMAKER.DB
        if (CurrentQuestionID == 1)    //Multiplier
        {
            if (thechoise == 0)
                Database.OrdenadosMultiplier = 2;
            else if (thechoise == 1)
                Database.OrdenadosMultiplier = 3;
            else if (thechoise == 2)
                Database.OrdenadosMultiplier = 4;

            //print("Multiplier: " + Database.OrdenadosMultiplier);
        }
        else if (CurrentQuestionID == 2)    //App Design
        {
            if (thechoise == 0)      //NADA
            {
                Database.AppDesign = 0;
                Database.StoresDesign = 0;
            }
            else if (thechoise == 1)      //We do everything
            {
                Database.AppDesign = 1;
                Database.StoresDesign = 1;
            }
            else if (thechoise == 2)      //We do only the app design
            {
                Database.AppDesign = 1;
                Database.StoresDesign = 0;
            }
            else if (thechoise == 3)      //We do only the stores design
            {
                Database.AppDesign = 0;
                Database.StoresDesign = 1;
            }

            //print("AppDesign: " + Database.AppDesign);
            //print("StoresDesign: " + Database.StoresDesign);
        }
    }

    #endregion

    #region Do Fase

    public void DoFase()
    {
        Current.text = "Current Time: " + ((int)(CurrentTime/8)) + " days; Current Budget: " + CurrentMoney + "€ + IVA";

        //USING DATABASE.CS
        //Title.text = Database.Titles[CurrentFase];        

        //USING BUDGETMAKER.DB
        if (CurrentQuestionID == 0)
        {
            Title.text = QuestionsTable.dataTable[0]["QuestionText"].ToString();
            CurrentQuestionID = 1;
        }
        else
            Title.text = QuestionsTable.dataTable[CurrentQuestionID - 1]["QuestionText"].ToString();

        #region Instantiate

        //USING DATABASE.CS
//        int count = 0;
//        for(int i = 0; i < 10; i++, count++)
//        {
//            if (Database.OptionsTexts[CurrentFase, i] == "" || Database.OptionsTexts[CurrentFase, i] == null)
//                break;
//        }


        //USING BUDGETMAKER.DB
        DBReader query = db.Select ("SELECT * FROM Answers WHERE Question_id = " + CurrentQuestionID);
        int count = query.dataTable.Count;

        int x = -700;
        int y = 130;
        for(int i = 0; i < count; i++)
        {
            #region USING DATABASE.CS
//            GameObject temp = Instantiate(ChoicePrefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
//            temp.GetComponentInChildren<ButtonAux>().ChoiseNumber = i;
//            temp.transform.FindChild("Text").GetComponentInChildren<Text>().text = Database.OptionsTexts[CurrentFase, i];
//            if(Database.OptionsPrices[CurrentFase, i].ToString() == "-8.8")
//                temp.transform.FindChild("Price").GetComponentInChildren<Text>().text = "...";
//            else
//                temp.transform.FindChild("Price").GetComponentInChildren<Text>().text = Database.OptionsPrices[CurrentFase, i].ToString() + "€";
//            temp.transform.SetParent(TheCanvas.transform);
            #endregion


            //USING BUDGETMAKER.DB
            GameObject temp = Instantiate(ChoicePrefab, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
            temp.GetComponentInChildren<ButtonAux>().ChoiseNumber = i;
            temp.transform.FindChild("Text").GetComponentInChildren<Text>().text = query.dataTable[i]["AnswerText"].ToString();

            if(query.dataTable[i]["Horas_Unity"].ToString() == "-1")
                temp.transform.FindChild("Price").GetComponentInChildren<Text>().text = "...";
            else if (query.dataTable[i]["Price"].ToString() != "")
                temp.transform.FindChild("Price").GetComponentInChildren<Text>().text = query.dataTable[i]["Price"].ToString() + "€";
            else
            {
//                print("1AppDesign: " + Database.AppDesign);
//                print("1StoresDesign: " + Database.StoresDesign);
//                print("1Multiplier: " + Database.OrdenadosMultiplier);
//                print("UnityTime: " + int.Parse(query.dataTable[i]["Horas_Unity"].ToString()));
//                print("WordPressTime: " + int.Parse(query.dataTable[i]["Horas_WordPress"].ToString()));
//                print("SQLTime: " + int.Parse(query.dataTable[i]["Horas_SQL"].ToString()));
//                print("ArtTime: " + int.Parse(query.dataTable[i]["Horas_Art"].ToString()));
//                print("StoreTime: " + int.Parse(query.dataTable[i]["Horas_Store"].ToString()));
//                print("OutrosTime: " + int.Parse(query.dataTable[i]["Horas_Outros"].ToString()));

                temp.transform.FindChild("Price").GetComponentInChildren<Text>().text = (int.Parse(query.dataTable[i]["Horas_Unity"].ToString()) + int.Parse(query.dataTable[i]["Horas_WordPress"].ToString())
                                                                                        + int.Parse(query.dataTable[i]["Horas_SQL"].ToString()) + (int.Parse(query.dataTable[i]["Horas_Art"].ToString()) * Database.AppDesign)
                                                                                        + (int.Parse(query.dataTable[i]["Horas_Store"].ToString()) * Database.StoresDesign) + int.Parse(query.dataTable[i]["Horas_Outros"].ToString()))
                                                                                            *4.4f*Database.OrdenadosMultiplier + "€";
            }

            temp.transform.SetParent(TheCanvas.transform);


            //Falta a image aqui

            if (x == 700)
            {
                x = -700;
                y = -260;
            }
            else
                x += 350;
        }

        #endregion
    }

    #endregion

    #region Do Back Fase

    public void DoBackFase () 
    {
        #region USING DATABASE.CS
//        if (CurrentFase <= 23 && CurrentFase != 0)
//        {
//            //print("Fase: " + CurrentFase + ". Back To: " + BackFase);
//
//            //Destroy AllPrevious Options
//            GameObject[] options = GameObject.FindGameObjectsWithTag("Options");
//            foreach (GameObject option in options)
//                Destroy(option);
//            
//            CurrentFase = (int)BackFaseStack.Pop();
//            CurrentTime -= (int)BackTimeStack.Pop();
//            CurrentMoney -= (float)BackMoneyStack.Pop();
//
//            if (CurrentFase <= Database.NumberSets - 1)
//                DoFase();
//        }
        #endregion

        //USING BUDGETMAKER.DB
        if (CurrentQuestionID <= QuestionsTable.dataTable.Count && CurrentQuestionID > 1)
        {
            GameObject[] options = GameObject.FindGameObjectsWithTag("Options");
            foreach (GameObject option in options)
                Destroy(option);

            CurrentQuestionID = (int)BackFaseStack.Pop();
            CurrentTime -= (int)BackTimeStack.Pop();
            CurrentUnityTime -= (int)BackUnityTimeStack.Pop();
            CurrentWordPressTime -= (int)BackWordPressTimeStack.Pop();
            CurrentSQLTime -= (int)BackSQLTimeStack.Pop();
            CurrentArtTime -= (int)BackArtTimeStack.Pop();
            CurrentStoreTime -= (int)BackStoreTimeStack.Pop();
            CurrentOutrosTime -= (int)BackOutrosTimeStack.Pop();
            CurrentMoney -= (float)BackMoneyStack.Pop();

            DoFase();
        }
    }

    #endregion

    #region Reset

    public void Reset () 
    {
        //Destroy AllPrevious Options
        GameObject[] options = GameObject.FindGameObjectsWithTag("Options");
        foreach (GameObject option in options)
            Destroy(option);

        CurrentQuestionID = 0;

        //CurrentFase = 0;
        CurrentTime = 0;          
        CurrentMoney = CurrentTime*4.4f*Database.OrdenadosMultiplier;
        OptionsImages = new Image[QuestionsTable.dataTable.Count + 1];
        OptionsTexts = new string[QuestionsTable.dataTable.Count + 1];
        OptionsPrice = new float[QuestionsTable.dataTable.Count + 1];
        OptionsUnityTime = new float[QuestionsTable.dataTable.Count + 1];
        OptionsWordPressTime = new float[QuestionsTable.dataTable.Count + 1];
        OptionsSQLTime = new float[QuestionsTable.dataTable.Count + 1];
        OptionsArtTime = new float[QuestionsTable.dataTable.Count + 1];
        OptionsStoreTime = new float[QuestionsTable.dataTable.Count + 1];
        OptionsOutrosTime = new float[QuestionsTable.dataTable.Count + 1];

        BackFaseStack.Clear();

        Results.text = "";
        FaturaText.text = "";

        Inputfield.SetActive(false);

        //Build First Fase
        DoFase();
    }

    #endregion

    #region Do The End

    string Fatura = "";
    string Unity = "";
    string WordPress = "";
    string SQL = "";
    string Art = "";
    string Store = "";
    string Outros = "";

    public void DoTheEnd()
    {
        Fatura = "";
        Unity = "";
        WordPress = "";
        SQL = "";
        Art = "";
        Store = "";
        Outros = "";

        //print("All The choices: ");
        for (int i = 0; i <= QuestionsTable.dataTable.Count; i++)
        {
            //print(i + ": " + OptionsTexts[i] + ": " + OptionsPrice[i] + "€");

            if (OptionsTexts[i] != "" && OptionsTexts[i] != null && OptionsPrice[i] != 0 && OptionsPrice[i] != -8.8f)
            {
                #region DATABASE.CS

//                if(Database.Titles[i] == "Main Menu")
//                    Fatura += "Main Menu: " + OptionsPrice[i] + "€\n";
//                else if(Database.Titles[i] == "Número de Menus diferentes\n(Excepto Main Menu)")
//                    Fatura += OptionsTexts[i] + " Ecrãs: " + OptionsPrice[i] + "€\n";
//                else if(Database.Titles[i] == "Total de Produtos/Pontos/etc importados por nós")
//                    Fatura += OptionsTexts[i] + " Produtos/Pontos/etc importados por nós: " + OptionsPrice[i] + "€\n";
//                else if(Database.Titles[i] == "Total de Back-offices para editar produtos/pontos/etc")
//                    Fatura += OptionsTexts[i] + " Back-offices de edição: " + OptionsPrice[i] + "€\n";
//                else if(Database.Titles[i] == "Total de Back-offices para Visualização de dados apenas")
//                    Fatura += OptionsTexts[i] + " Back-offices view: " + OptionsPrice[i] + "€\n";
//                else if(Database.Titles[i] == "Georreferenciação?")
//                    Fatura += "Geo " + OptionsTexts[i] + ": " + OptionsPrice[i] + "€\n";
//                else if(Database.Titles[i] == "Número de tipos de pontos diferentes de georreferenciação?")
//                    Fatura += OptionsTexts[i] + " tipos de pontos geo: " + OptionsPrice[i] + "€\n";
//                else if(Database.Titles[i] == "Número de QR Codes")
//                    Fatura += OptionsTexts[i] + " QR Codes: " + OptionsPrice[i] + "€\n";
//                else if(Database.Titles[i] == "Compras na aplicação?")
//                    Fatura += "InApps: " + OptionsPrice[i] + "€\n";
//                else if(Database.Titles[i] == "Publicidade AdMob?")
//                    Fatura += "AdMob: " + OptionsPrice[i] + "€\n";
//                else if(Database.Titles[i] == "Número de Linguas")
//                    Fatura += OptionsTexts[i] + " Linguas: " + OptionsPrice[i] + "€\n";
//                else if(Database.Titles[i] == "Contactos?\n(Email / Telefone / WebSite)")
//                    Fatura += "Contactos: " + OptionsPrice[i] + "€\n";
//                else if(Database.Titles[i] == "Favoritos?")
//                    Fatura += "Favoritos: " + OptionsPrice[i] + "€\n";
//                else if(Database.Titles[i] == "Historico?")
//                    Fatura += "Histórico: " + OptionsPrice[i] + "€\n";
//                else if(Database.Titles[i] == "Share Global?")
//                    Fatura += "Share Global: " + OptionsPrice[i] + "€\n";
//                else if(Database.Titles[i] == "Algum tipo de login?")
//                    Fatura += "Login " + OptionsTexts[i] + ": " + OptionsPrice[i] + "€\n";
//                else if(Database.Titles[i] == "Requer informação dos amigos do Facebook?")
//                    Fatura += "Facebook friends: " + OptionsPrice[i] + "€\n";
//                else if(Database.Titles[i] == "Sistema de Comentarios?")
//                    Fatura += "Sistema de comentários: " + OptionsPrice[i] + "€\n";
//                else if(Database.Titles[i] == "Analytics?")
//                    Fatura += "Analytics: " + OptionsPrice[i] + "€\n";
//                else if(Database.Titles[i] == "Número de Fases/Reuniões")
//                    Fatura += OptionsTexts[i] + " Fases/Reuniões: " + OptionsPrice[i] + "€\n";
//                else if(Database.Titles[i] == "Quantos dias de art?")
//                    Fatura += OptionsTexts[i] + " dias de art: " + OptionsPrice[i] + "€\n";
//                else if(Database.Titles[i] == "Quantos dias de program?")
//                    Fatura += OptionsTexts[i] + " dias de programming: " + OptionsPrice[i] + "€\n";
//                else
//                    Fatura += OptionsTexts[i] + ": " + OptionsPrice[i] + "€\n";

                #endregion

                if (QuestionsTable.dataTable[i - 1]["QuestionText"].ToString() == "Main Menu")
                    SaveAndWriteAnswer("Main Menu: ", i);
                else if (QuestionsTable.dataTable[i - 1]["QuestionText"].ToString() == "Número de Menus diferentes\n(Excepto Main Menu)")
                    SaveAndWriteAnswer(OptionsTexts[i] + " Ecrãs: ", i);
                else if (QuestionsTable.dataTable[i - 1]["QuestionText"].ToString() == "Total de Produtos/Pontos/etc importados por nós")
                    SaveAndWriteAnswer(OptionsTexts[i] + " 'cenas' importadas por nós: ", i);
                else if (QuestionsTable.dataTable[i - 1]["QuestionText"].ToString() == "Total de Back-offices para editar produtos/pontos/etc")
                    SaveAndWriteAnswer(OptionsTexts[i] + " Back-offices de edição: ", i);
                else if (QuestionsTable.dataTable[i - 1]["QuestionText"].ToString() == "Total de Back-offices para Visualização de dados apenas")
                    SaveAndWriteAnswer(OptionsTexts[i] + " Back-offices view: ", i);
                else if (QuestionsTable.dataTable[i - 1]["QuestionText"].ToString() == "Georreferenciação?")
                    SaveAndWriteAnswer("Geo " + OptionsTexts[i] + ": ", i);
                else if (QuestionsTable.dataTable[i - 1]["QuestionText"].ToString() == "Número de tipos de pontos diferentes de georreferenciação?")
                    SaveAndWriteAnswer(OptionsTexts[i] + " tipos de pontos geo: ", i);
                else if (QuestionsTable.dataTable[i - 1]["QuestionText"].ToString() == "Número de QR Codes")
                    SaveAndWriteAnswer(OptionsTexts[i] + " QR Codes: ", i);
                else if (QuestionsTable.dataTable[i - 1]["QuestionText"].ToString() == "Compras na aplicação?")
                    SaveAndWriteAnswer("InApps: ", i);
                else if (QuestionsTable.dataTable[i - 1]["QuestionText"].ToString() == "Publicidade AdMob?")
                    SaveAndWriteAnswer("AdMob: ", i);
                else if (QuestionsTable.dataTable[i - 1]["QuestionText"].ToString() == "Número de Linguas")
                    SaveAndWriteAnswer(OptionsTexts[i] + " Linguas: ", i);
                else if (QuestionsTable.dataTable[i - 1]["QuestionText"].ToString() == "Contactos?\n(Email / Telefone / WebSite)")
                    SaveAndWriteAnswer("Contactos: ", i);
                else if(QuestionsTable.dataTable[i - 1]["QuestionText"].ToString() == "Favoritos?")
                    SaveAndWriteAnswer("Favoritos: ", i);
                else if(QuestionsTable.dataTable[i - 1]["QuestionText"].ToString() == "Historico?")
                    SaveAndWriteAnswer("Histórico: ", i);
                else if(QuestionsTable.dataTable[i - 1]["QuestionText"].ToString() == "Share Global?")
                    SaveAndWriteAnswer("Share Global: ", i);
                else if(QuestionsTable.dataTable[i - 1]["QuestionText"].ToString() == "Algum tipo de login?")
                    SaveAndWriteAnswer("Login " + OptionsTexts[i] + ": ", i);
                else if(QuestionsTable.dataTable[i - 1]["QuestionText"].ToString() == "Requer informação dos amigos do Facebook?")
                    SaveAndWriteAnswer("Facebook friends: ", i);
                else if(QuestionsTable.dataTable[i - 1]["QuestionText"].ToString() == "Sistema de Comentarios?")
                    SaveAndWriteAnswer("Sistema de comentários: ", i);
                else if(QuestionsTable.dataTable[i - 1]["QuestionText"].ToString() == "Analytics?")
                    SaveAndWriteAnswer("Analytics: ", i);
                else if(QuestionsTable.dataTable[i - 1]["QuestionText"].ToString() == "Número de Fases/Reuniões")
                    SaveAndWriteAnswer(OptionsTexts[i] + " Fases/Reuniões: ", i);
                else
                    SaveAndWriteAnswer(OptionsTexts[i] + ": ", i);
            }

            if(i > 20 && QuestionsTable.dataTable[i - 1]["QuestionText"].ToString() == "Gamify?")
                SaveAndWriteAnswer(OptionsTexts[i] + " Gamify: ", i);
        }

        //print("");
        //print("Tempo: " + ((int)(CurrentTime/8) + 1) + " days");     //+1 in case some hours left
        //print("Orçamento: " + CurrentMoney + "€ + IVA");

        Title.text = "";
        Inputfield.SetActive(true);
        //Results.text = "Tempo: " + ((int)(CurrentTime/8) + 1) + " days\n" + "Orçamento: " + CurrentMoney + "€ + IVA";
        Results.text = "Tempo: " + ((int)(CurrentTime/8)) + " days\n" + "Orçamento: " + CurrentMoney + "€ + IVA";            //WHY +1????
        FaturaText.text = Fatura;
        System.IO.File.WriteAllText(Application.persistentDataPath + "\\Fatura.txt", Fatura + "\n" + Results.text);


        #region Planeamento

        string Planeamento = "";
        if(Database.AppDesign == 1)
            Planeamento += "Design da App: " + CurrentArtTime + " hours -> " + (CurrentArtTime/8.0f) + " days" + Art;
        
        Planeamento += "\n\nFazer a App: " + CurrentUnityTime + " hours -> " + (CurrentUnityTime/8.0f) + " days" + Unity;
        Planeamento += "\n\nDatabase Comunication: " + CurrentSQLTime + " hours -> " + (CurrentSQLTime/8.0f) + " days" + SQL;

        if(Database.StoresDesign == 1)
            Planeamento += "\n\nPublicação da App: " + CurrentStoreTime + " hours -> " + (CurrentStoreTime/8.0f) + " days" + Store;
        
        Planeamento += "\n\nBack-offices: " + CurrentWordPressTime + " hours -> " + (CurrentWordPressTime/8.0f) + " days" + WordPress;
        Planeamento += "\n\nOther Time: " + CurrentOutrosTime + " hours -> " + (CurrentOutrosTime/8.0f) + " days" + Outros;

        System.IO.File.WriteAllText(Application.persistentDataPath + "\\Planeamento.txt", Planeamento);

        #endregion
    }

    public void SaveAndWriteAnswer(string QuestionText, int i)
    {
        Fatura += QuestionText + OptionsPrice[i] + "€\n";

        if(OptionsUnityTime[i] != 0 && OptionsUnityTime[i] != -1)
            Unity += "\n - " + QuestionText + OptionsUnityTime[i] + " hours -> " + (OptionsUnityTime[i] / 8.0f) + " days";

        if(OptionsWordPressTime[i] != 0 && OptionsWordPressTime[i] != -1)
            WordPress += "\n - " + QuestionText + OptionsWordPressTime[i] + " hours -> " + (OptionsWordPressTime[i] / 8.0f) + " days";

        if(OptionsSQLTime[i] != 0 && OptionsSQLTime[i] != -1)
            SQL += "\n - " + QuestionText + OptionsSQLTime[i] + " hours -> " + (OptionsSQLTime[i] / 8.0f) + " days";

        if(OptionsArtTime[i] != 0 && OptionsArtTime[i] != -1)
            Art += "\n - " + QuestionText + OptionsArtTime[i] + " hours -> " + (OptionsArtTime[i] / 8.0f) + " days";

        if(OptionsStoreTime[i] != 0 && OptionsStoreTime[i] != -1)
            Store += "\n - " + QuestionText + OptionsStoreTime[i] + " hours -> " + (OptionsStoreTime[i] / 8.0f) + " days";

        if(OptionsOutrosTime[i] != 0 && OptionsOutrosTime[i] != -1)
            Outros += "\n - " + QuestionText + OptionsOutrosTime[i] + " hours -> " + (OptionsOutrosTime[i] / 8.0f) + " days";
    }

    #endregion
}
