using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.Linq;
using System.Linq.Expressions;

namespace ObjectCompare
{
    public class CompareDbObjects<T> where T : class
    {
        DataContext _sourceDb;
        DataContext _targetDb;
        public CompareDbObjects(IDbConnection prodConnection, IDbConnection devConnection)
        {
            _sourceDb = new DataContext(prodConnection);
            _targetDb = new DataContext(devConnection);
        }

        public CompareModel<T> CompareObjectsValues(CompareModel<T> stuff, TextWriter outputStream=null)
        {
            CompareModel<T> newStuff = new CompareModel<T>();
            string html = "";
            T source = stuff.SourceObject;
            T target = stuff.TargetObject;

            source.GetType().GetProperties().ToList().ForEach( p =>
            {
                if(!stuff.ExemptProperties.Contains(p.Name))
                {
                    if(!Equals(p.GetValue(source), target.GetType().GetProperty(p.Name).GetValue(target)))
                    {
                        stuff.MismatchedProperties.Add(p.Name, source.GetType().Name);
                        if(outputStream != null)
                        {
                            outputStream.WriteLine(string.Format("{0}:", p.Name));
                            outputStream.WriteLine(string.Format("Source - {0}", p.GetValue(source)));
                            outputStream.WriteLine(string.Format("Target - {0}", target.GetType().GetProperty(p.Name).GetValue(target)));
                            outputStream.WriteLine(outputStream.NewLine);
                        }
                    }
                }
            });

            if(stuff.SubObjects.Count > 0)
            {
                stuff.SubObjects.ForEach(p =>
                {
                    var sourceSub = source.GetType().GetProperty(p);
                    var targetSub = target.GetType().GetProperty(p);
                    sourceSub.GetType().GetProperties().ToList().ForEach(pp =>
                    {
                        
                        if (!stuff.ExemptProperties.Contains(pp.Name))
                        {
                            if (targetSub.GetType().GetProperty(pp.Name) != null)
                            {
                                if (!Equals(
                                    pp.GetValue(sourceSub),
                                    targetSub.GetType().GetProperty(pp.Name).GetValue(targetSub)))
                                {
                                    stuff.MismatchedProperties.Add(pp.Name, p);
                                    if (outputStream != null)
                                    {
                                        outputStream.WriteLine(string.Format("{0}:", pp.Name));
                                        outputStream.WriteLine(string.Format("Source - {0}", pp.GetValue(sourceSub)));
                                        outputStream.WriteLine(string.Format("Target - {0}", targetSub.GetType().GetProperty(pp.Name).GetValue(targetSub)));
                                        outputStream.WriteLine(outputStream.NewLine);
                                    }
                                }
                            }
                        }
                    });
                });
            }

            newStuff = stuff;
            return newStuff;
        }

        public CompareModel<T> CompareOneToOne(List<string> exemptProperties, List<string> subObjects, Expression<Func<T, bool>> sourceExpr, Expression<Func<T, bool>> targetExpr, TextWriter outputStream=null)
        {
            CompareModel<T> data = new CompareModel<T>();
            data.ExemptProperties = exemptProperties;
            data.SubObjects = subObjects;
            data.SourceObject = _sourceDb.GetTable<T>().Where(sourceExpr).FirstOrDefault();
            data.TargetObject = _targetDb.GetTable<T>().Where(targetExpr).FirstOrDefault();
            return CompareObjectsValues(data, outputStream);
        }
    }
}
