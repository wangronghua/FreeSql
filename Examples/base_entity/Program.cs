﻿using FreeSql;
using FreeSql.DataAnnotations;
using FreeSql.Extensions;
using FreeSql.Internal.CommonProvider;
using FreeSql.Internal.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace base_entity
{
    class Program
    {
        class TestConfig
        {
            public int clicks { get; set; }
            public string title { get; set; }
        }
        [Table(Name = "sysconfig")]
        public class S_SysConfig<T> : BaseEntity<S_SysConfig<T>>
        {
            [Column(IsPrimary = true)]
            public string Name { get; set; }

            [JsonMap]
            public T Config { get; set; }

            public T Config2 { get; set; }
        }

        public class Products : BaseEntity<Products, int>
        {
            public string title { get; set; }
            public int testint { get; set; }
        }

        static AsyncLocal<IUnitOfWork> _asyncUow = new AsyncLocal<IUnitOfWork>();

        public class TestEnumCls
        {
            public CollationTypeEnum val { get; set; } = CollationTypeEnum.Binary;
        }

        class Sys_reg_user
        {
            public Guid Id { get; set; }
            public Guid OwnerId { get; set; }
            public string UnionId { get; set; }

            [Navigate(nameof(OwnerId))]
            public Sys_owner Owner { get; set; }
        }
        class Sys_owner
        {
            public Guid Id { get; set; }
            public Guid RegUserId { get; set; }

            [Navigate(nameof(RegUserId))]
            public Sys_reg_user RegUser { get; set; }
        }

        public class tttorder
        {
            [Column(IsPrimary = true)]
            public long Id { get; set; }
            public string Title { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }


            public tttorder(string title, int quantity, decimal price)
            {
                Id = DateTime.Now.Ticks;
                Title = title;
                Quantity = quantity;
                Price = price;
            }
        }

        class B
        {
            public long Id { get; set; }
        }

        class A
        {
            public long BId { get; set; }
            public B B { get; set; }
        }

        [Table(Name = "as_table_log_{yyyyMM}", AsTable = "createtime=2022-1-1(1 month)")]
        class AsTableLog
        {
            public Guid id { get; set; }
            public string msg { get; set; }
            public DateTime createtime { get; set; }
        }

        static void Main(string[] args)
        {
            #region 初始化 IFreeSql
            var fsql = new FreeSql.FreeSqlBuilder()
                .UseAutoSyncStructure(true)
                .UseNoneCommandParameter(true)

                .UseConnectionString(FreeSql.DataType.Sqlite, "data source=test1.db;max pool size=5")
                //.UseSlave("data source=test1.db", "data source=test2.db", "data source=test3.db", "data source=test4.db")
                //.UseSlaveWeight(10, 1, 1, 5)


                //.UseConnectionString(FreeSql.DataType.MySql, "Data Source=127.0.0.1;Port=3306;User ID=root;Password=root;Initial Catalog=cccddd;Charset=utf8;SslMode=none;Max pool size=2")

                //.UseConnectionString(FreeSql.DataType.SqlServer, "Data Source=.;Integrated Security=True;Initial Catalog=freesqlTest;Pooling=true;Max Pool Size=3")

                //.UseConnectionString(FreeSql.DataType.PostgreSQL, "Host=192.168.164.10;Port=5432;Username=postgres;Password=123456;Database=tedb;Pooling=true;Maximum Pool Size=2")
                //.UseNameConvert(FreeSql.Internal.NameConvertType.ToLower)

                //.UseConnectionString(FreeSql.DataType.Oracle, "user id=user1;password=123456;data source=//127.0.0.1:1521/XE;Pooling=true;Max Pool Size=2")
                //.UseNameConvert(FreeSql.Internal.NameConvertType.ToUpper)


                //.UseConnectionString(FreeSql.DataType.OdbcMySql, "Driver={MySQL ODBC 8.0 Unicode Driver};Server=127.0.0.1;Persist Security Info=False;Trusted_Connection=Yes;UID=root;PWD=root;DATABASE=cccddd_odbc;Charset=utf8;SslMode=none;Max pool size=2")

                //.UseConnectionString(FreeSql.DataType.OdbcSqlServer, "Driver={SQL Server};Server=.;Persist Security Info=False;Trusted_Connection=Yes;Integrated Security=True;DATABASE=freesqlTest_odbc;Pooling=true;Max pool size=3")

                //.UseConnectionString(FreeSql.DataType.OdbcPostgreSQL, "Driver={PostgreSQL Unicode(x64)};Server=192.168.164.10;Port=5432;UID=postgres;PWD=123456;Database=tedb_odbc;Pooling=true;Maximum Pool Size=2")
                //.UseNameConvert(FreeSql.Internal.NameConvertType.ToLower)

                //.UseConnectionString(FreeSql.DataType.OdbcOracle, "Driver={Oracle in XE};Server=//127.0.0.1:1521/XE;Persist Security Info=False;Trusted_Connection=Yes;UID=odbc1;PWD=123456")
                //.UseNameConvert(FreeSql.Internal.NameConvertType.ToUpper)

                //.UseConnectionString(FreeSql.DataType.OdbcDameng, "Driver={DM8 ODBC DRIVER};Server=127.0.0.1:5236;Persist Security Info=False;Trusted_Connection=Yes;UID=USER1;PWD=123456789")

                .UseMonitorCommand(null, (umcmd, log) => Console.WriteLine(umcmd.Connection.ConnectionString + ":" + umcmd.CommandText))
                .UseLazyLoading(true)
                .UseGenerateCommandParameterWithLambda(true)
                .Build();
            BaseEntity.Initialization(fsql, () => _asyncUow.Value);
            #endregion

var testitems = new[]
{
    new AsTableLog{ msg = "msg01", createtime = DateTime.Parse("2022-1-1 13:00:11") },
    new AsTableLog{ msg = "msg02", createtime = DateTime.Parse("2022-1-2 14:00:12") },
    new AsTableLog{ msg = "msg03", createtime = DateTime.Parse("2022-2-2 15:00:13") },
    new AsTableLog{ msg = "msg04", createtime = DateTime.Parse("2022-2-8 15:00:13") },
    new AsTableLog{ msg = "msg05", createtime = DateTime.Parse("2022-3-8 15:00:13") },
    new AsTableLog{ msg = "msg06", createtime = DateTime.Parse("2022-4-8 15:00:13") },
    new AsTableLog{ msg = "msg07", createtime = DateTime.Parse("2022-6-8 15:00:13") },
    new AsTableLog{ msg = "msg07", createtime = DateTime.Parse("2022-7-1") }
};
var sqlatb = fsql.Insert(testitems).NoneParameter();
var sqlat = sqlatb.ToSql();
var sqlatr = sqlatb.ExecuteAffrows();

            var sqlatc = fsql.Delete<AsTableLog>().Where(a => a.id == Guid.NewGuid() && a.createtime.Between(DateTime.Parse("2022-3-1"), DateTime.Parse("2022-5-1")));
            var sqlatca = sqlatc.ToSql();
            var sqlatcr = sqlatc.ExecuteAffrows();

            var sqlatd1 = fsql.Update<AsTableLog>().SetSource(testitems[0]);
            var sqlatd101 = sqlatd1.ToSql();
            var sqlatd102 = sqlatd1.ExecuteAffrows();

            var sqlatd2 = fsql.Update<AsTableLog>().SetSource(testitems[5]);
            var sqlatd201 = sqlatd2.ToSql();
            var sqlatd202 = sqlatd2.ExecuteAffrows();

            var sqlatd3 = fsql.Update<AsTableLog>().SetSource(testitems);
            var sqlatd301 = sqlatd3.ToSql();
            var sqlatd302 = sqlatd3.ExecuteAffrows();

            var sqlatd4 = fsql.Update<AsTableLog>(Guid.NewGuid()).Set(a => a.msg == "newmsg");
            var sqlatd401 = sqlatd4.ToSql();
            var sqlatd402 = sqlatd4.ExecuteAffrows();

            var sqlatd5 = fsql.Update<AsTableLog>(Guid.NewGuid()).Set(a => a.msg == "newmsg").Where(a => a.createtime.Between(DateTime.Parse("2022-3-1"), DateTime.Parse("2022-5-1")));
            var sqlatd501 = sqlatd5.ToSql();
            var sqlatd502 = sqlatd5.ExecuteAffrows();

            var sqlatd6 = fsql.Update<AsTableLog>(Guid.NewGuid()).Set(a => a.msg == "newmsg").Where(a => a.createtime > DateTime.Parse("2022-3-1") && a.createtime < DateTime.Parse("2022-5-1"));
            var sqlatd601 = sqlatd6.ToSql();
            var sqlatd602 = sqlatd6.ExecuteAffrows();

            var sqlatd7 = fsql.Update<AsTableLog>(Guid.NewGuid()).Set(a => a.msg == "newmsg").Where(a => a.createtime > DateTime.Parse("2022-3-1"));
            var sqlatd701 = sqlatd7.ToSql();
            var sqlatd702 = sqlatd7.ExecuteAffrows();

            var sqlatd8 = fsql.Update<AsTableLog>(Guid.NewGuid()).Set(a => a.msg == "newmsg").Where(a => a.createtime < DateTime.Parse("2022-5-1"));
            var sqlatd801 = sqlatd8.ToSql();
            var sqlatd802 = sqlatd8.ExecuteAffrows();

            var sqls1 = fsql.Select<AsTableLog>();
            var sqls101 = sqls1.ToSql();
            var sqls102 = sqls1.ToList();

            var sqls2 = fsql.Select<AsTableLog>().Where(a => a.createtime.Between(DateTime.Parse("2022-3-1"), DateTime.Parse("2022-5-1")));
            var sqls201 = sqls2.ToSql();
            var sqls202 = sqls2.ToList();

            var sqls3 = fsql.Select<AsTableLog>().Where(a => a.createtime > DateTime.Parse("2022-3-1") && a.createtime < DateTime.Parse("2022-5-1"));
            var sqls301 = sqls3.ToSql();
            var sqls302 = sqls3.ToList();

            var sqls4 = fsql.Select<AsTableLog>().Where(a => a.createtime > DateTime.Parse("2022-3-1"));
            var sqls401 = sqls4.ToSql();
            var sqls402 = sqls4.ToList();

            var sqls5 = fsql.Select<AsTableLog>().Where(a => a.createtime < DateTime.Parse("2022-5-1"));
            var sqls501 = sqls5.ToSql();
            var sqls502 = sqls5.ToList();

            fsql.Aop.AuditValue += new EventHandler<FreeSql.Aop.AuditValueEventArgs>((_, e) =>
            {
                
            });

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("id", 1);
            dic.Add("name", "xxxx");
            var diclist = new List<Dictionary<string, object>>();
            diclist.Add(dic);
            diclist.Add(new Dictionary<string, object>
            {
                ["id"] = 2,
                ["name"] = "yyyy"
            });

            var sqss = fsql.InsertDict(dic).AsTable("table1").ToSql();
            var sqss2 = fsql.InsertDict(diclist).AsTable("table1").ToSql();
            sqss = fsql.InsertDict(dic).AsTable("table1").NoneParameter(false).ToSql();
            sqss2 = fsql.InsertDict(diclist).AsTable("table1").NoneParameter(false).ToSql();

            var sqlupd1 = fsql.UpdateDict(dic).AsTable("table1").WherePrimary("id").ToSql();
            var sqlupd2 = fsql.UpdateDict(diclist).AsTable("table1").WherePrimary("id").ToSql();
            var sqlupd11 = fsql.UpdateDict(dic).AsTable("table1").WherePrimary("id").NoneParameter(false).ToSql();
            var sqlupd22 = fsql.UpdateDict(diclist).AsTable("table1").WherePrimary("id").NoneParameter(false).ToSql();

            var sqldel1 = fsql.DeleteDict(dic).AsTable("table1").ToSql();
            var sqldel2 = fsql.DeleteDict(diclist).AsTable("table1").ToSql();
            diclist[1]["title"] = "newtitle";
            var sqldel3 = fsql.DeleteDict(diclist).AsTable("table1").ToSql();
            diclist.Clear();
            diclist.Add(new Dictionary<string, object>
            {
                ["id"] = 1
            });
            diclist.Add(new Dictionary<string, object>
            {
                ["id"] = 2
            });
            var sqldel4 = fsql.DeleteDict(diclist).AsTable("table1").ToSql();


            for (var a = 0; a < 10000; a++)
                fsql.Select<User1>().First();

            for (var a = 0; a < 1000; a++)
            {
                fsql.Transaction(() =>
                {
                    var tran = fsql.Ado.TransactionCurrentThread;
                    tran.Rollback();
                });
            }

            fsql.UseJsonMap();
            var bid1 = 10;
            var list1 = fsql.Select<A>()
                .Where(a => a.BId == bid1);
            var aid1 = 11;
            var select2 = fsql.Select<B>();
            (select2 as Select0Provider)._params = (list1 as Select0Provider)._params;
            var list2 = select2
                .Where(a => list1.ToList(B => B.BId).Contains(a.Id))
                .Where(a => a.Id == aid1)
                .ToSql();

            //fsql.Aop.CommandBefore += (s, e) =>
            //{
            //    e.States["xxx"] = 111;
            //};
            //fsql.Aop.CommandAfter += (s, e) =>
            //{
            //    var xxx = e.States["xxx"];
            //};

            //fsql.Aop.TraceBefore += (s, e) =>
            //{
            //    e.States["xxx"] = 222;
            //};
            //fsql.Aop.TraceAfter += (s, e) =>
            //{
            //    var xxx = e.States["xxx"];
            //};

            //fsql.Aop.SyncStructureBefore += (s, e) =>
            //{
            //    e.States["xxx"] = 333;
            //};
            //fsql.Aop.SyncStructureAfter += (s, e) =>
            //{
            //    var xxx = e.States["xxx"];
            //};

            //fsql.Aop.CurdBefore += (s, e) =>
            //{
            //    e.States["xxx"] = 444;
            //};
            //fsql.Aop.CurdAfter += (s, e) =>
            //{
            //    var xxx = e.States["xxx"];
            //};

            fsql.Insert(new tttorder("xx1", 1, 10)).ExecuteAffrows();
            fsql.Insert(new tttorder("xx2", 2, 20)).ExecuteAffrows();

            var tttorders = fsql.Select<tttorder>().Limit(2).ToList();

            var tsql1 = fsql.Select<Sys_reg_user>()
                .Include(a => a.Owner)
                .Where(a => a.UnionId == "xxx")
                .ToSql();
            var tsql2 = fsql.Select<Sys_owner>()
                .Where(a => a.RegUser.UnionId == "xxx2")
                .ToSql();


            var names = (fsql.Select<object>() as Select0Provider)._commonUtils.SplitTableName("`Backups.ProductStockBak`");


            var dbparams = fsql.Ado.GetDbParamtersByObject(new { id = 1, name = "xxx" });





            var sql = fsql.CodeFirst.GetComparisonDDLStatements(typeof(EMSServerModel.Model.User), "testxsx001");

            var test01 = EMSServerModel.Model.User.Select.IncludeMany(a => a.Roles).ToList();
            var test02 = EMSServerModel.Model.UserRole.Select.ToList();
            var test01tb = EMSServerModel.Model.User.Orm.CodeFirst.GetTableByEntity(typeof(EMSServerModel.Model.User));

            var us = User1.Select.Limit(10).ToList();

            new Products { title = "product-1" }.Save();
            new Products { title = "product-2" }.Save();
            new Products { title = "product-3" }.Save();
            new Products { title = "product-4" }.Save();
            new Products { title = "product-5" }.Save();

            var wdy1 = JsonConvert.DeserializeObject<DynamicFilterInfo>(@"
{
  ""Logic"" : ""And"",
  ""Filters"" :
  [
    {
      ""Logic"" : ""Or"",
      ""Filters"" :
      [
        {
          ""Field"" : ""title"",
          ""Operator"" : ""contains"",
          ""Value"" : """",
        },
        {
          ""Field"" : ""title"",
          ""Operator"" : ""contains"",
          ""Value"" : ""product-2222"",
        }
      ]
    },
    {
      ""Field"" : ""title"",
      ""Operator"" : ""eq"",
      ""Value"" : ""product-2""
    },
    {
      ""Field"" : ""title"",
      ""Operator"" : ""eq"",
      ""Value"" : ""product-3""
    },
    {
      ""Field"" : ""title"",
      ""Operator"" : ""eq"",
      ""Value"" : ""product-4""
    },
    {
      ""Field"" : ""testint"",
      ""Operator"" : ""Range"",
      ""Value"" : [100,200]
    },
    {
      ""Field"" : ""testint"",
      ""Operator"" : ""Range"",
      ""Value"" : [""101"",""202""]
    },
    {
      ""Field"" : ""testint"",
      ""Operator"" : ""contains"",
      ""Value"" : ""123""
    },
  ]
}
"); 
            var config = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = null,
                AllowTrailingCommas = true,
                IgnoreNullValues = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                Converters = { new JsonStringEnumConverter() }
            };
            var wdy2 = System.Text.Json.JsonSerializer.Deserialize<DynamicFilterInfo>(@"
{
  ""Logic"" : 1,
  ""Filters"" :
  [
    {
      ""Field"" : ""title"",
      ""Operator"" : 8,
      ""Value"" : ""product-1"",
      ""Filters"" :
      [
        {
          ""Field"" : ""title"",
          ""Operator"" : 0,
          ""Value"" : ""product-1111""
        }
      ]
    },
    {
      ""Field"" : ""title"",
      ""Operator"" : 8,
      ""Value"" : ""product-2""
    },
    {
      ""Field"" : ""title"",
      ""Operator"" : 8,
      ""Value"" : ""product-3""
    },
    {
      ""Field"" : ""title"",
      ""Operator"" : 8,
      ""Value"" : ""product-4""
    },
    {
      ""Field"" : ""testint"",
      ""Operator"" : 8,
      ""Value"" : 11
    },
    {
      ""Field"" : ""testint"",
      ""Operator"" : 8,
      ""Value"" : ""12""
    },
    {
      ""Field"" : ""testint"",
      ""Operator"" : ""Range"",
      ""Value"" : [100,200]
    },
    {
      ""Field"" : ""testint"",
      ""Operator"" : ""Range"",
      ""Value"" : [""101"",""202""]
    }
  ]
}
", config);
            Products.Select.WhereDynamicFilter(wdy1).ToList();
            Products.Select.WhereDynamicFilter(wdy2).ToList();

            var items1 = Products.Select.Limit(10).OrderByDescending(a => a.CreateTime).ToList();
            var items2 = fsql.Select<Products>().Limit(10).OrderByDescending(a => a.CreateTime).ToList();

            BaseEntity.Orm.UseJsonMap();
            BaseEntity.Orm.UseJsonMap();
            BaseEntity.Orm.CodeFirst.ConfigEntity<S_SysConfig<TestConfig>>(a =>
            {
                a.Property(b => b.Config2).JsonMap();
            });

            new S_SysConfig<TestConfig> { Name = "testkey11", Config = new TestConfig { clicks = 11, title = "testtitle11" }, Config2 = new TestConfig { clicks = 11, title = "testtitle11" } }.Save();
            new S_SysConfig<TestConfig> { Name = "testkey22", Config = new TestConfig { clicks = 22, title = "testtitle22" }, Config2 = new TestConfig { clicks = 11, title = "testtitle11" } }.Save();
            new S_SysConfig<TestConfig> { Name = "testkey33", Config = new TestConfig { clicks = 33, title = "testtitle33" }, Config2 = new TestConfig { clicks = 11, title = "testtitle11" } }.Save();
            var testconfigs11 = S_SysConfig<TestConfig>.Select.ToList();
            var testconfigs11tb = S_SysConfig<TestConfig>.Select.ToDataTable();
            var testconfigs111 = S_SysConfig<TestConfig>.Select.ToList(a => a.Name);
            var testconfigs112 = S_SysConfig<TestConfig>.Select.ToList(a => a.Config);
            var testconfigs1122 = S_SysConfig<TestConfig>.Select.ToList(a => new { a.Name, a.Config });
            var testconfigs113 = S_SysConfig<TestConfig>.Select.ToList(a => a.Config2);
            var testconfigs1133 = S_SysConfig<TestConfig>.Select.ToList(a => new { a.Name, a.Config2 });

            var repo = BaseEntity.Orm.Select<TestConfig>().Limit(10).ToList();


            //void ConfigEntityProperty(object sender, FreeSql.Aop.ConfigEntityPropertyEventArgs e)
            //{
            //    if (e.Property.PropertyType == typeof(byte[]))
            //    {
            //        var orm = sender as IFreeSql;
            //        switch (orm.Ado.DataType)
            //        {
            //            case DataType.SqlServer:
            //                e.ModifyResult.DbType = "image";
            //                break;
            //            case DataType.MySql:
            //                e.ModifyResult.DbType = "longblob";
            //                break;
            //        }
            //    }
            //}
            //fsql.Aop.ConfigEntityProperty += ConfigEntityProperty;


            Task.Run(async () =>
            {
                using (var uow = BaseEntity.Orm.CreateUnitOfWork())
                {
                    _asyncUow.Value = uow;
                    try
                    {
                        var id = (await new User1().SaveAsync()).Id;
                    }
                    finally
                    {
                        _asyncUow.Value = null;
                    }
                    uow.Commit();
                }

                var ug1 = new UserGroup();
                ug1.GroupName = "分组一";
                await ug1.InsertAsync();

                var ug2 = new UserGroup();
                ug2.GroupName = "分组二";
                await ug2.InsertAsync();

                var u1 = new User1();

                u1.GroupId = ug1.Id;
                await u1.SaveAsync();

                await u1.DeleteAsync();
                await u1.RestoreAsync();

                u1.Nickname = "x1";
                await u1.UpdateAsync();

                var u11 = await User1.FindAsync(u1.Id);
                u11.Description = "备注";
                await u11.SaveAsync();

                await u11.DeleteAsync();

                var slslsl = Newtonsoft.Json.JsonConvert.SerializeObject(u1);
                var u11null = User1.Find(u1.Id);

                var u11s = User1.Where(a => a.Group.Id == ug1.Id).Limit(10).ToList();

                var u11s2 = User1.Select.LeftJoin<UserGroup>((a, b) => a.GroupId == b.Id).Limit(10).ToList();

                var ug1s = UserGroup.Select
                    .IncludeMany(a => a.User1s)
                    .Limit(10).ToList();

                var ug1s2 = UserGroup.Select.Where(a => a.User1s.AsSelect().Any(b => b.Nickname == "x1")).Limit(10).ToList();

                var r1 = new Role();
                r1.Id = "管理员";
                await r1.SaveAsync();

                var r2 = new Role();
                r2.Id = "超级会员";
                await r2.SaveAsync();

                var ru1 = new RoleUser1();
                ru1.User1Id = u1.Id;
                ru1.RoleId = r1.Id;
                await ru1.SaveAsync();

                ru1.RoleId = r2.Id;
                await ru1.SaveAsync();

                var u1roles = await User1.Select.IncludeMany(a => a.Roles).ToListAsync();
                var u1roles2 = await User1.Select.Where(a => a.Roles.AsSelect().Any(b => b.Id == "xx")).ToListAsync();

            }).Wait();

            

            Console.WriteLine("按任意键结束。。。");
            Console.ReadKey();
        }
    }
}
