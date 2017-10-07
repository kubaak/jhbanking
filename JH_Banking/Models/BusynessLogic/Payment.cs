using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace JH_Banking.Models.BusynessLogic
{
    public class Payment
    {
        public JH_DBContext db = new JH_DBContext();
        public bool makePayment(Transaction tr)
        {
            using (var tran = db.Database.BeginTransaction())
            {
                db.BankAccount.Where(ba => ba.AccNum == tr.AccNum).FirstOrDefault().Balance -= tr.Amount;
                if (db.BankAccount.Where(ba => ba.AccNum == tr.AccNum).FirstOrDefault().Balance >= 0)
                {
                    db.Transaction.Add(tr);
                    if (tr.DestBankCode == ConfigurationManager.AppSettings["bankCode".ToString()])
                    {
                        db.BankAccount.Where(ba => ba.AccNum == tr.DestAccNum).FirstOrDefault().Balance += tr.Amount;
                    }
                    db.SaveChanges();
                    tran.Commit();
                    return true;
                }
                else
                {
                    tran.Rollback();
                    return false;
                }                               
            }            
        }
    }
}