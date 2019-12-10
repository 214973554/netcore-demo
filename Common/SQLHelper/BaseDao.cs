using Dapper;
using log4net;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Common
{
    public class BaseDao
    {
        #region 成员变量
        /// <summary>
        /// 默认连接key
        /// </summary>
        private static string _Connect = "LoanDB";
        protected Log4netHelper log;
        protected IConfiguration configuration;

        private Stopwatch sw = new Stopwatch();

        private int dueTime = 10 * 1000;
        #endregion

        public BaseDao(IConfiguration configuration, ILog ilog)
        {
            this.configuration = configuration;
            this.log = new Log4netHelper(ilog, this.GetType());
        }

        #region 获取数据连接
        /// <summary>
        /// 获取数据连接
        /// </summary>
        /// <returns></returns>
        protected virtual SqlConnection GetConnection()
        {
            return GetConnection(_Connect);
        }

        /// <summary>
        /// 获取数据连接
        /// </summary>
        /// <param name="connectKey"></param>
        /// <returns></returns>
        protected virtual SqlConnection GetConnection(string connectKey)
        {
            string connectionString = configuration[$"Vcredit:connectionStrings:{connectKey}"];
            SqlConnection conn = new SqlConnection(connectionString);
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            return conn;
        }
        #endregion

        #region 获取数据通用
        protected virtual List<T> Query<T>(string sql, IDictionary<string, object> param, string connectKey = "", CommandType commandType = CommandType.Text, int timeOut = 0)
        {

            connectKey = connectKey.Length > 0 ? connectKey : _Connect;

            var args = new DynamicParameters();
            if (param != null)
            {
                foreach (KeyValuePair<string, object> kvp in param)
                {
                    args.Add(kvp.Key, kvp.Value);
                }
            }
            using (var conn = GetConnection(connectKey))
            {
                try
                {
                    sw.Reset();
                    sw.Start();

                    List<T> result = conn.Query<T>(sql, args, null, true, timeOut,
                           commandType).ToList();

                    return result;
                }
                catch (Exception ex)
                {
                    log.Log(LogSql(sql, args), LogType.Error, ex);
                    return default(List<T>);
                }
                finally
                {
                    sw.Stop();
                    if (sw.ElapsedMilliseconds > dueTime)
                    {
                        log.Log(LogSql(sql, args) + "耗时:" + sw.ElapsedMilliseconds + "毫秒", LogType.Warn);
                    }
                }
            }
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="sql">执行脚本</param>
        /// <param name="param">输入参数</param>
        /// <param name="outPutParam">输出参数</param>
        /// <param name="connectKey">连接key</param>
        /// <param name="commandType">执行类型</param>
        /// <returns></returns>
        protected virtual List<T> Query<T>(string sql, IDictionary<string, object> param, ref IDictionary<string, object> outPutParam, string connectKey = "", CommandType commandType = CommandType.Text, int timeOut = 0)
        {
            connectKey = connectKey.Length > 0 ? connectKey : _Connect;

            var args = new DynamicParameters();
            if (param != null)
            {
                foreach (KeyValuePair<string, object> kvp in param)
                {
                    args.Add(kvp.Key, kvp.Value);
                }
            }
            if (outPutParam != null)
            {
                foreach (KeyValuePair<string, object> kvp in outPutParam)
                {
                    args.Add(kvp.Key, kvp.Value, null, ParameterDirection.Output);
                }
            }
            using (var conn = GetConnection(connectKey))
            {
                try
                {
                    sw.Reset();
                    sw.Start();

                    List<T> result = conn.Query<T>(sql, args, null, true, timeOut,
                           commandType).ToList();

                    if (outPutParam != null)
                    {
                        IDictionary<string, object> outPut = new Dictionary<string, object>();
                        foreach (string key in outPutParam.Keys)
                        {
                            outPut.Add(key, args.Get<object>(key));
                        }
                        outPutParam = outPut;
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    log.Log(LogSql(sql, args), LogType.Error, ex);

                    return default(List<T>);
                }
                finally
                {
                    sw.Stop();
                    if (sw.ElapsedMilliseconds > dueTime)
                    {
                        log.Log(LogSql(sql, args) + "耗时:" + sw.ElapsedMilliseconds + "毫秒", LogType.Warn);
                    }
                }
            }
        }


        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="sql">执行脚本</param>
        /// <param name="param">参数</param>
        /// <param name="connectKey">连接key</param>
        /// <param name="commandType">执行类型</param>
        /// <returns></returns>
        protected virtual int Execute(string sql, IDictionary<string, object> param, string connectKey = "", CommandType commandType = CommandType.Text)
        {
            connectKey = connectKey.Length > 0 ? connectKey : _Connect;

            var args = new DynamicParameters();
            if (param != null)
            {
                foreach (KeyValuePair<string, object> kvp in param)
                {
                    args.Add(kvp.Key, kvp.Value);
                }
            }
            using (var conn = GetConnection(connectKey))
            {
                try
                {
                    sw.Reset();
                    sw.Start();
                    return conn.Execute(sql, args, null, null,
                           commandType);
                }
                catch (Exception ex)
                {
                    log.Log(LogSql(sql, args), LogType.Error, ex);

                    return default(int);
                }
                finally
                {
                    sw.Stop();
                    if (sw.ElapsedMilliseconds > dueTime)
                    {
                        log.Log(LogSql(sql, args) + "耗时:" + sw.ElapsedMilliseconds + "毫秒", LogType.Warn);
                    }
                }
            }
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="sql">执行脚本</param>
        /// <param name="param">参数</param>
        /// <param name="connectKey">连接key</param>
        /// <param name="commandType">执行类型</param>
        /// <returns></returns>
        protected virtual int ExecuteCt(string sql, IDictionary<string, object> param, ref IDictionary<string, object> outPutParam, string connectKey = "", CommandType commandType = CommandType.Text, int timeout = 0)
        {
            connectKey = connectKey.Length > 0 ? connectKey : _Connect;

            var args = new DynamicParameters();
            if (param != null)
            {
                foreach (KeyValuePair<string, object> kvp in param)
                {
                    args.Add(kvp.Key, kvp.Value);
                }
            }
            if (outPutParam != null)
            {
                foreach (KeyValuePair<string, object> kvp in outPutParam)
                {
                    args.Add(kvp.Key, kvp.Value, null, ParameterDirection.Output);
                }
            }
            using (var conn = GetConnection(connectKey))
            {
                try
                {
                    sw.Reset();
                    sw.Start();
                    int result = conn.Execute(sql, args, null, timeout, commandType);

                    if (outPutParam != null)
                    {
                        IDictionary<string, object> outPut = new Dictionary<string, object>();
                        foreach (string key in outPutParam.Keys)
                        {
                            outPut.Add(key, args.Get<object>(key));
                        }
                        outPutParam = outPut;
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    log.Log(LogSql(sql, args), LogType.Error, ex);

                    return default(int);
                }
                finally
                {
                    sw.Stop();
                    if (sw.ElapsedMilliseconds > dueTime)
                    {
                        log.Log(LogSql(sql, args) + "耗时:" + sw.ElapsedMilliseconds + "毫秒", LogType.Warn);
                    }
                }
            }
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="sql">执行脚本</param>
        /// <param name="param">参数</param>
        /// <param name="connectKey">连接key</param>
        /// <param name="commandType">执行类型</param>
        /// <returns></returns>
        protected virtual int Execute(string sql, IDictionary<string, object> param, ref IDictionary<string, object> outPutParam, string connectKey = "", CommandType commandType = CommandType.Text, int timeout = 0)
        {
            connectKey = connectKey.Length > 0 ? connectKey : _Connect;

            var args = new DynamicParameters();
            if (param != null)
            {
                foreach (KeyValuePair<string, object> kvp in param)
                {
                    args.Add(kvp.Key, kvp.Value);
                }
            }
            if (outPutParam != null)
            {
                foreach (KeyValuePair<string, object> kvp in outPutParam)
                {
                    args.Add(kvp.Key, kvp.Value, null, ParameterDirection.Output);
                }
            }
            using (var conn = GetConnection(connectKey))
            {
                try
                {
                    sw.Reset();
                    sw.Start();
                    int result = conn.Execute(sql, args, null, timeout,
                           commandType);

                    if (outPutParam != null)
                    {
                        IDictionary<string, object> outPut = new Dictionary<string, object>();
                        foreach (string key in outPutParam.Keys)
                        {
                            outPut.Add(key, args.Get<object>(key));
                        }
                        outPutParam = outPut;
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    log.Log(LogSql(sql, args), LogType.Error, ex);

                    return default(int);
                }
                finally
                {
                    sw.Stop();
                    if (sw.ElapsedMilliseconds > dueTime)
                    {
                        log.Log(LogSql(sql, args) + "耗时:" + sw.ElapsedMilliseconds + "毫秒", LogType.Warn);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="connectKey"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        protected virtual object QueryScalar(string sql, IDictionary<string, object> param, string connectKey = "", CommandType commandType = CommandType.Text, int timeout = 0)
        {
            connectKey = connectKey.Length > 0 ? connectKey : _Connect;

            SqlParameter[] sqlParameter = null;

            if (param != null)
            {
                sqlParameter = new SqlParameter[param.Count];
                int i = 0;
                foreach (KeyValuePair<string, object> kvp in param)
                {
                    sqlParameter.SetValue(new SqlParameter(kvp.Key, kvp.Value), i);
                    i++;
                }
            }

            var conn =  GetConnection(connectKey);
            try
            {
                sw.Reset();
                sw.Start();
                return conn.ExecuteScalar(sql, sqlParameter, null, timeout, commandType);
            }
            catch (Exception ex)
            {
                log.Log(LogSql(sql, sqlParameter), LogType.Error, ex);

                return default(DataSet);
            }
            finally
            {
                sw.Stop();
                if (sw.ElapsedMilliseconds > dueTime)
                {
                    log.Log(LogSql(sql, sqlParameter) + "耗时:" + sw.ElapsedMilliseconds + "毫秒", LogType.Warn);
                }
            }
        }
        #endregion

        #region 数据库日志
        private string LogSql(string sql, DynamicParameters args)
        {
            var param = args.ParameterNames.Select(o => string.Format("参数:{0},值:{1}", o, args.Get<object>(o)));
            return string.Format("执行脚本:{0}{1}{2}", sql, Environment.NewLine, string.Join("   ", param));
        }

        private string LogSql(string sql, SqlParameter[] args)
        {
            var param = args.Select(o => string.Format("参数:{0},值:{1}", o.ParameterName, o.Value));
            return string.Format("执行脚本:{0}{1}{2}", sql, Environment.NewLine, string.Join("   ", param));
        }
        #endregion

        /// <summary>
        /// 获取字符串连接的数据库
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public string GetDB(string db)
        {
            string result = "";
            using (var sqlConn = GetConnection(db))
            {
                result = sqlConn.Database;
            }
            return result;
        }

        /// <summary>
        /// 批量更新数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <param name="fields"></param>
        /// <param name="tableName">表名</param>
        /// <param name="key">主键</param>
        /// <returns></returns>
        public bool BatchUpdate<T>(List<T> entities, string[] fields, string tableName, string key)
        {
            StringBuilder sql = new StringBuilder();
            Dictionary<string, object> param = new Dictionary<string, object>();
            for (int i = 0; i < entities.Count; i++)
            {
                T l = entities[i];
                sql.Append(" update " + tableName + " set ");
                string strP = string.Empty;
                foreach (string f in fields)
                {
                    strP += f + "=@" + f + i + ",";
                    param.Add("@" + f + i, l.GetType().GetProperty(f).GetValue(l, null));
                }
                sql.Append(strP.Substring(0, strP.Length - 1));
                sql.Append(" where " + key + "=@" + key + i + " ; ");
                param.Add("@" + key + i, l.GetType().GetProperty(key).GetValue(l, null));
            }
            bool result = Execute(sql.ToString(), param, "",
                   CommandType.Text) > 0;
            return result;
        }
    }
}