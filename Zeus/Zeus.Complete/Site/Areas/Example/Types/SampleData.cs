using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Employment.Web.Mvc.Area.Example.Types
{
    public static class SampleData
    {
        public static IEnumerable<SelectListItem> FillDictionary()
        {
            var list = new List<SelectListItem>();
            {

                var selectListItemToAdd = new SelectListItem { Selected = true };
                selectListItemToAdd.Text = "(Dan) Thanh Tung Vo (TTVO)";
                selectListItemToAdd.Value = "(Dan) Thanh Tung Vo (TTVO)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = "Staff Services Pty Ltd (OOAB)";
                selectListItemToAdd.Value = "Staff Services Pty Ltd (OOAB)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "3D PTY LTD (YYDT)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "ATSI Corporation For Community Development (QUPE)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "A.C.N. 105 864 006 Pty Ltd (KKDB)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "A.D.P. Recruitment Services (Aust) Pty Ltd (KKCE)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "A1 Employment and Training Services Pty Ltd (AAAB)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "A4e Austraa (VVCH)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Aallstaff Resources Pty Ltd (FFCD)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "ABC Corporate Development (XXCO)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Abity Options mited (BBJB)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Abity Tasmania Group Inc (NNCY)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Abity Technology (TTDR)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Employment Group Pty Ltd (BBAX)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Aboriginal Business Consultants (WWDT)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "Aboriginal Connections Employment Services (VVCI)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "Aboriginal Corp of Employ  Train Develop Inc Rauk (HHCW)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "Aboriginal Corporation of Emp  Tng Development (BBET)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "Aboriginal Employment Brokers Pty Ltd (FFDM)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LlpurrurulLm Fommunity Government FounFil (GGHG)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LlternLtive Networks Pty LtJ (LLTE)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Lltus Employment ServiFes Pty LtJ (GGFH)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Lltus LLGour Hire Pty LtJ (SSMI)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LM:PM Fonsulting Pty LimiteJ (GGJG)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LML ServiFes (WL) Pty LtJ (LTGY)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LmLtL Fommunity InF (HHLF)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LmGiilmunguNgLrrL LGoriginLl ForporLtion (FFFT)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LmGrose InJigenous Gusiness (FFJT)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LMML Org (LMM)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LmoongunL Fommunity InForporLteJ (HHLH)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LnForw FoOperLtive LtJ (LNFO)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LnJrew H West  LssoFiLtes (GGJL)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LngliFLn Youth  EJuFLtion JioFese of SyJney(LNGL)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LngliFLre LgeLJvLntLge (FFJG)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LngliFLre FLnGerrL LnJ GoulGurn (MMJK)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LngliFLre NT (NNNT)  ";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LngliFLre SL (LLFR)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LngliFLre TLsmLniL InF (LTLS)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LngliFLre Top EnJ (TTTO)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LngliFLre ViFtoriL (FFLJ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LngliFLre WL InF. (TTFG)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "Lngurugu Fommunity Government FounFil InF (IILM)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LnilLlyL HomelLnJs FounFil LGoriginLl ForporLtion(KKLG)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LnmLtjere Fommunity Government FounFil (LNML)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LnmLtjere Fommunity Government FounFil (FFLP)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LnneFto InF (GGMJ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "Lnnerley  JistriFt Fommunity Fentre TSU(LJFF)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LnsL Solutions Pty LtJ (FFLS)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LpprentiFes TrLinees Employment LtJ (LWLM)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LpprentiFeships LustrLliL (YYFH)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LpprentiFeships ViFtoriL (KFSU)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LPS  PLFifiF Pty LtJ (GGLM)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LPS Group (HolJings) Pty LtJ (GGHO)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LputulL Housing LssoFiLtion InF (MMLL)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "'LquL Lgri Enterprises Pty LtJ (GGEJ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LRL JoGs Pty LtJ (LUSR)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LreyongL Fommunity InForporLteJ (GGFE)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LrltLrlpiltL Fommunity Government FounFil (OOLM)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LrmLJLle NoongLr ForporLtion (LGoriginLl Forp)(IILJ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LrmiJLle  JistriFt Gusiness Enterprise FentreLtJ (LJGE)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LrmiJLle Employment LGoriginLl ForporLtion (QQLP)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Lrmstrong Muller Fonsulting Pty LtJ (XXFG)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Lrrow ReFruitment (GGFJ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Lrtius (GGFS)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LSLP ReFruitment Pty LtJ (TTFS)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LsFenJenFe Pty LtJ (LSFE)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LsFet People (FFHW)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LSeTTS (LSTT)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LshGurton LGoriginLl ForporLtion (RRLJ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LSK Employment LnJ TrLining ServiFes (LJUL)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "Lspire HumLn FLpitLl MLnLgement Pty LimiteJ (GGFG)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Lssessments LustrLliL (TTJP)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LssoFiLtion for the GlinJ of WL (InF.) (TTJN)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LsstoFk Pty LtJ (REGI)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LsteriL ServiFes InF (GGME)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Lstute ServiFes Pty LtJ (GGLT)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Ltkinson Kerr LnJ LssoFiLtes Pty LtJ (KKEG)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Ltwork LustrLliL Pty LtJ (WKFF)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LuGurn FLreer TrLining ServiFes InF (POSI)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LurorL PrLFtiFLl Solutions Pty LimiteJ (GJGQ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Lurukun Shire FounFil (SSLN)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Lus WorkforFe Pty LtJ (GGWF)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LusFom TrLining ServiFes Pty LtJ (HHEF)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LusInfo  Government InformLtion for LustrLliLns(LUSI)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LUSkey Org (LUS)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Luslink MLnLgement FonsultLnts (IIJO)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Lussie GLtewLys (WWJQ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LustFhinL TeleFom ServiFes Pty LtJ (GGTS)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLlLsiLn Follege GroLJwLy The LustrLlLsiLnFol (KKEW)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLlLsiLn MLritime Institute Pty LtJ (OOFR)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLlLsiLn MeLt InJustry Employees Union (RRJH)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LustrLliL Personnel GloGLl Pty LtJ (GGLP)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn Gusiness Jevelopment Fentre (QFZG)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LustrLliLn Gusiness TLlent (LUSG)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn FLtholiF University LimiteJ (MMJM)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LUSTRLLILN FHILJ FLRE FLREER OPTIONS (QQFW)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn FhilJren's Trust Ls trustee for LustrLl(ITEM)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn FounFil for EJuFLtionLl ReseLrFh Limite(KKEF)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn EJuFLtion InJustry Fentre (LEIF) (LEIF)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn FlexiGle LeLrning Institute Pty LtJ(GGPT)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn Greek WelfLre SoFiety LimiteJ (LUSG)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LustrLliLn Hotels LssoFLtion (UUJK)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn InJigenous Gusiness ServiFes Pty LtJ(IIJF)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn InJigenous EJuFLtion FounJLtion (WWJR)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn InJigenous LeLJership Fentre (SSJL)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn InJigenous Mentoring ExperienFe (KKEX)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn IntegrLteJ Employment LnJ TrLiningServ (VVFO)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn MeJiFLl PlLFements Pty LtJ  JoJLzPty (GGFY)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LustrLliLn NLtionLl University (LUNU)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LustrLliLn Nursing LgenFy Pty LtJ (GGFG)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn QuLJriplegiF LssoFiLtion LtJ (LQLW)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LustrLliLn ReFruiting Pty LtJ (FFFR)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn ReFruitment FonsultLnts Pty LtJ (GGLQ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LustrLliLn ReJ Fross (LRFQ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn ReJeployment ServiFes Pty LtJ (GGLS)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LUSTRLLILN FHILJ FLRE FLREER OPTIONS (QQFW)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn FhilJren's Trust Ls trustee for LustrLl(ITEM)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn FounFil for EJuFLtionLl ReseLrFh Limite(KKEF)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LustrLliLn EJuFLtion InJustry Fentre (LEIF)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn FlexiGle LeLrning Institute Pty LtJ(GGPT)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn Greek WelfLre SoFiety LimiteJ (LUSG)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LustrLliLn Hotels LssoFLtion (UUJK)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn InJigenous Gusiness ServiFes Pty LtJ(IIJF)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn InJigenous EJuFLtion FounJLtion (WWJR)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn InJigenous LeLJership Fentre (SSJL)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn InJigenous Mentoring ExperienFe (KKEX)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn IntegrLteJ Employment LnJ TrLiningServ (VVFO)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn MeJiFLl PlLFements Pty LtJ  JoJLzPty (GGFY)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LustrLliLn NLtionLl University (LUNU)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LustrLliLn Nursing LgenFy Pty LtJ (GGFG)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn QuLJriplegiF LssoFiLtion LtJ (LQLW)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LustrLliLn ReFruiting Pty LtJ (FFFR)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn ReFruitment FonsultLnts Pty LtJ (GGLQ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LustrLliLn ReJ Fross (LRFQ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn ReJeployment ServiFes Pty LtJ (GGLS)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn Refugee LssoFiLtion InForporLteJ (GGEG)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn RetLilers LssoFiLtion  SL Jivision(GGFJ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn SFhool of HortiFulture Pty LtJ (IILS)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn TeFhnology PLrk SyJney LTJ (LTPSL)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LustrLliLn TrLining LlliLnFe Pty LtJ (XXFM)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LustrLliLn TrLining FompLny (FFHL)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn TrLining MLnLgement Pty LtJ (HHEU)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn VietnLmese Women's WelfLre LssoF InF(GGVW)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLn VietnLmese Women's WelfLre LssoFiLtion(MITE)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LustrLliLn WilJlife ServiFes (IIJJ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LustrLliLn WorkplLFe TrLining (OOFH)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LustrLliLnVietnLmese ServiFes ResourFe Fentre       InF (GGSR)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LustseLrFh ReFruitment Group Pty LtJ (GGLU)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LustwLy InternLtionLl (YYLS)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LuswiJe ProjeFts (STEP)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "Lutism LssoFiLtion Of South LustrLliL InF (GGMF)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "Lutism LssoFiLtion Of Western LustrLliL InF (GGMG)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "Lvon Fommunity Employment Support Fentre InF(GGMH)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LvonJLle Lsset Pty LtJ (GGLT)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LW Workwise Pty LimiteJ (GGLW)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "LwLre Skills ProjeFt (LWLR)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "LZTeFh ReFruitment LnJ FontrLFting Pty LtJ (GGTR)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "G J ElJreJ (IIFO)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "G.E.S.T. FonsultLnts (FFOL)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "GLGLnL LGoriginLl Men's Group InForporLteJ (WWJS)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GLF Fonsulting Group (YYFI)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GLkLs Employment Solutions (GLKL)";
                list.Add(selectListItemToAdd);


                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GLlLnix Solutions Pty LtJ (XXJP)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GLlfour Fonsulting (GGJQ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GLlgL JoG Link InF (GLLG)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GLlgL JoG Link InF (FFLG)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GLLKLNU (IIJS)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "GLllLrLt LnJ JistriFt LGoriginLl FooperLtive(GGEW)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GLllLrLt RegionLl InJustries InF (GLRI)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "GLmL NgLppi NgLppi LGoriginLl ForporLtion (GGNN)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "GLmL NgLppi NgLppi LGoriginLl ForporLtion (GMSU)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GLmLngL GuGu NgLJimunku InF (GGHG)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "GLnkstown Employment Skills TrLining InF (GLNK)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GLptist FLre InF (WEGL)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GLrkly Shire FounFil (SSFX)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GLrossL Enterprises InF (GGGK)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GLrrLh FJEP InForporLteJ (GGGP)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GLrriekneLl Housing  Fommunity LtJ (GGHJ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GLrry Smith  LssoFiLtes Pty LtJ (SMIT)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GLthun Pty LtJ (GGGX)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GLthurst ReFruitment (GGJG)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GLwinLngL LGoriginLl ForporLtion (GGHE)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GLYSL LtJ (FFLK)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "GLysiJe LJolesFent GoLrJing InF (G.L.G.I.) (GQFU)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "GLysiJe Employment Skills TrLining InF (GLYS)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GLysiJe Personnel LustrLliL Pty LtJ (SSJX)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GLysiJe Personnel Pty LtJ (GGPP)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GLysiJe South LustrLliL Pty LtJ (SSJY)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GLyteF Enterprises InF (PREF)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GLyteFh InJustriLl VIF Pty LtJ (SSJW)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GGI Group Pty LimiteJ (GGFW)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GFL NLtionLl TrLining Group Pty LtJ (GGER)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GJO (QLJ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Pty LtJ (XXJQ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GJO OrgLnisLtion Jevelopment (SL)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Pty LtJ (FFJL)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GJS People Pty LtJ (GGJS)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GeLuJesert TrLining Fentre InF (GELU)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GEF Western NEIS ServiFes (GEFW)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GeJforJ WorkforFe (GREE)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GeeLF Progress LssoFiLtion InF. TSU (GEEF)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "Geenleigh Housing LnJ Jevelopment Fo LtJ (ELGL)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "Gellingen Shire Enterprise Support TeLm InF (GEST)(TSEG)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GenFhmLrk ReFruitment (VIF) Pty LtJ (XXRR)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GenJigo LFFess Employment InF (GGEN)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GenJigo Fommunity HeLlth ServiFes (OOJF)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GengooJ Pty. LimiteJ (GGGY)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Gensons Group Pty LtJ (GENS)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Gerry Street ViFtoriL InForporLteJ (FFLL)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Gest FJG Pty LtJ (UUJO)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GEST Fommunity Jevelopment (GLLE)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GEST Employment LimiteJ (GRTJ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "Gest EnJeLvours Employment  TrLining Solutions(RRFH)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Gest MLtFh ReFruitment (OOFE)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Gest PrLFtiFe Skills Pty LtJ (RRPM)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "Getterlink Gusiness FonsultLnFy LnJ TrLining Servi (XXFE)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "GeyonJ GillLGong (YYFX)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "F J ElJreJ (IIFO)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "F.E.S.T. FonsultLnts (FFOL)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "FLFLnL LForiginLl Men's Group InForporLteJ (WWJS)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FLF Fonsulting Group (YYFI)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FLkLs Employment Solutions (FLKL)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FLlLnix Solutions Pty LtJ (XXJP)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FLlfour Fonsulting (FFJQ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FLlgL JoF Link InF (FLLG)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FLlgL JoF Link InF (FFLG)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FLLKLNU (IIJS)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "FLllLrLt LnJ JistriFt LForiginLl FooperLtive(GGEW)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FLllLrLt RegionLl InJustries InF (FLRI)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "FLmL NgLppi NgLppi LForiginLl ForporLtion (FFNN)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "FLmL NgLppi NgLppi LForiginLl ForporLtion (FMSU)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FLmLngL FuFu NgLJimunku InF (FFHF)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "FLnkstown Employment Skills TrLining InF (FLNK)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FLptist FLre InF (WEFL)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FLrkly Shire FounFil (SSFX)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FLrossL Enterprises InF (FFFK)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FLrrLh FJEP InForporLteJ (FFFP)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FLrriekneLl Housing  Fommunity LtJ (FFHJ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FLrry Smith  LssoFiLtes Pty LtJ (SMIT)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FLthun Pty LtJ (FFFX)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FLthurst ReFruitment (GGJF)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FLwinLngL LForiginLl ForporLtion (FFHE)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FLYSL LtJ (FFLK)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "FLysiJe LJolesFent FoLrJing InF (F.L.F.I.) (FQFU)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "FLysiJe Employment Skills TrLining InF (FLYS)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FLysiJe Personnel LustrLliL Pty LtJ (SSJX)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FLysiJe Personnel Pty LtJ (FFPP)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FLysiJe South LustrLliL Pty LtJ (SSJY)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FLyteF Enterprises InF (PREF)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FLyteFh InJustriLl VIF Pty LtJ (SSJW)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FFI Group Pty LimiteJ (FFFW)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FFL NLtionLl TrLining Group Pty LtJ (FFER)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FJO (QLJ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Pty LtJ (XXJQ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FJO OrgLnisLtion Jevelopment (SL)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Pty LtJ (FFJL)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FJS People Pty LtJ (FFJS)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FeLuJesert TrLining Fentre InF (FELU)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FEF Western NEIS ServiFes (FEFW)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FeJforJ WorkforFe (FREE)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FeeLF Progress LssoFiLtion InF. TSU (FEEF)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "Feenleigh Housing LnJ Jevelopment Fo LtJ (ELGL)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "Fellingen Shire Enterprise Support TeLm InF (FEST)(TSEF)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FenFhmLrk ReFruitment (VIF) Pty LtJ (XXRR)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FenJigo LFFess Employment InF (FFEN)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FenJigo Fommunity HeLlth ServiFes (OOJF)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FengooJ Pty. LimiteJ (FFFY)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Fensons Group Pty LtJ (FENS)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Ferry Street ViFtoriL InForporLteJ (FFLL)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Fest FJG Pty LtJ (UUJO)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FEST Fommunity Jevelopment (FLLE)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FEST Employment LimiteJ (FRTJ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "Fest EnJeLvours Employment  TrLining Solutions(RRFH)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Fest MLtFh ReFruitment (OOFE)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Fest PrLFtiFe Skills Pty LtJ (RRPM)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "Fetterlink Fusiness FonsultLnFy LnJ TrLining Servi (XXFE)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "FeyonJ FillLFong (YYFX)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "J J ElJreJ (IIFO)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "J.E.S.T. FonsultLnts (FFOL)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "JLJLnL LJoriginLl Men's Group InForporLteJ (WWJS)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JLF Fonsulting Group (YYFI)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JLkLs Employment Solutions (JLKL)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JLlLnix Solutions Pty LtJ (XXJP)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JLlfour Fonsulting (JJJQ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JLlgL JoJ Link InF (JLLG)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JLlgL JoJ Link InF (FFLG)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JLLKLNU (IIJS)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "JLllLrLt LnJ JistriFt LJoriginLl FooperLtive(GGEW)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JLllLrLt RegionLl InJustries InF (JLRI)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "JLmL NgLppi NgLppi LJoriginLl ForporLtion (JJNN)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "JLmL NgLppi NgLppi LJoriginLl ForporLtion (JMSU)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JLmLngL JuJu NgLJimunku InF (JJHJ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "JLnkstown Employment Skills TrLining InF (JLNK)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JLptist FLre InF (WEJL)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JLrkly Shire FounFil (SSFX)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JLrossL Enterprises InF (JJJK)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JLrrLh FJEP InForporLteJ (JJJP)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JLrriekneLl Housing  Fommunity LtJ (JJHJ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JLrry Smith  LssoFiLtes Pty LtJ (SMIT)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JLthun Pty LtJ (JJJX)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JLthurst ReFruitment (GGJJ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JLwinLngL LJoriginLl ForporLtion (JJHE)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JLYSL LtJ (FFLK)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "JLysiJe LJolesFent JoLrJing InF (J.L.J.I.) (JQFU)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "JLysiJe Employment Skills TrLining InF (JLYS)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JLysiJe Personnel LustrLliL Pty LtJ (SSJX)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JLysiJe Personnel Pty LtJ (JJPP)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JLysiJe South LustrLliL Pty LtJ (SSJY)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JLyteF Enterprises InF (PREF)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JLyteFh InJustriLl VIF Pty LtJ (SSJW)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JJI Group Pty LimiteJ (JJFW)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JFL NLtionLl TrLining Group Pty LtJ (JJER)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JJO (QLJ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Pty LtJ (XXJQ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JJO OrgLnisLtion Jevelopment (SL)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Pty LtJ (FFJL)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JJS People Pty LtJ (JJJS)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JeLuJesert TrLining Fentre InF (JELU)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JEF Western NEIS ServiFes (JEFW)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JeJforJ WorkforFe (JREE)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JeeLF Progress LssoFiLtion InF. TSU (JEEF)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "Jeenleigh Housing LnJ Jevelopment Fo LtJ (ELGL)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "Jellingen Shire Enterprise Support TeLm InF (JEST)(TSEJ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JenFhmLrk ReFruitment (VIF) Pty LtJ (XXRR)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JenJigo LFFess Employment InF (JJEN)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JenJigo Fommunity HeLlth ServiFes (OOJF)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JengooJ Pty. LimiteJ (JJJY)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Jensons Group Pty LtJ (JENS)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Jerry Street ViFtoriL InForporLteJ (FFLL)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Jest FJG Pty LtJ (UUJO)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JEST Fommunity Jevelopment (JLLE)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "JEST Employment LimiteJ (JRTJ)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text =
                    selectListItemToAdd.Value = "Jest EnJeLvours Employment  TrLining Solutions(RRFH)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Jest MLtFh ReFruitment (OOFE)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = selectListItemToAdd.Value = "technology services employment (RRPM)";
                list.Add(selectListItemToAdd);





                selectListItemToAdd = new SelectListItem();
                selectListItemToAdd.Text = "Aboriginal Employment Strategy (AESL)";
                selectListItemToAdd.Value = "Aboriginal Employment Strategy (AESL)";
                list.Add(selectListItemToAdd);
                selectListItemToAdd = new SelectListItem { };
                selectListItemToAdd.Text = selectListItemToAdd.Value = "Pty Ltd (OOAB)";
                list.Add(selectListItemToAdd);

            }
            return list;
        }

    }
}