using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace SporOzet.Controllers
{
    public class HaberController : Controller
    {
        // GET: Haber
        public ActionResult Index()
        {  // spor haberlerinin Id si alınıyor
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            String deger = wc.DownloadString("https://api.hurriyet.com.tr/v1/articles?$select=Id&$filter=Path eq '/spor/'&$top=8&apikey=b145647c334249fd8504d1ccbb895374");
            JArray gelenId = JArray.Parse(deger);
            String[] id = new string[8];
            // spor haberlerinin Id si alındı ve id[] dizisine atanıyor
            for (int i = 0; i < 8; i++)
            {
                id[i] = (String)gelenId[i]["Id"];
            }

            // çekilen Idlere göre haberler çekiliyor
            JObject[] haberler = new JObject[8];
            for (int i = 0; i < 8; i++)
            {
                String gelenHaber = wc.DownloadString("https://api.hurriyet.com.tr/v1/articles/" + id[i] + "?apikey=b145647c334249fd8504d1ccbb895374");
                JObject news = JObject.Parse(gelenHaber);
                haberler[i] = news;
            }
            ViewBag.haberler = haberler;
            return View();
        }

        //özet çıkarma metodu
        public int[] ozetle(String haber)
        {
            int sıra = 0;
            String[] cumle = haber.Split('.', '?'); //haber metni burada cümlelere ayrılıyor
            string[] onemli = { "şok", "derbi", "malup", "yendi", "kazandı", "\"", "sonuç", "galip", "önemli", "açıklama yaptı", "açıklandı", "açıkladı", "berabere", "ifade edildi", "özetle", "kısaca", "sonuç" };
            string[] onemsiz = { "geçmiş", "boş işler", "çünkü", "çünki", "ancak", "öyle" };
            int[] onemSırası = new int[cumle.Length];
            //cümleler içinde dolaşıp puanlama yapılır
            foreach (var cum in cumle)
            {
                //uzun cümleler için puan kırılır
                if (cumle[sıra].Length > 15)
                {
                    onemSırası[sıra]--;
                }
                //ilk cümleler puan alır
                if (sıra < 5)
                {
                    onemSırası[sıra]++;
                }
                //son cümleler puan kazanır
                if (sıra > cumle.Length - 5)
                {
                    onemSırası[sıra]++;
                }
                //önemli kelimeler aranır ve puanı artar
                foreach (var onem in onemli)
                {
                    if (cum.IndexOf(onem) != -1)
                    {
                        onemSırası[sıra]++;
                    }
                }
                //önemsiz kelimeler bulunur ve puanı kırılır
                foreach (var onem in onemsiz)
                {
                    if (cum.IndexOf(onem) != -1)
                    {
                        onemSırası[sıra]--;
                    }
                }
                sıra++;//bir sonraki cümleye geçilir
            }
            return onemSırası;
        }
    }
}