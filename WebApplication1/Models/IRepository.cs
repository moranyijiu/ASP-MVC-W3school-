using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public interface IRepository : IDisposable
    {

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Entity"></param>
        /// <returns></returns>
        bool Add<T>(T Entity) where T : class;

        /// <summary>
        /// 批量的进行添加实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Entity"></param>
        /// <returns></returns>
        bool AddRange<T>(List<T> Entity) where T : class;


        /// <summary>
        /// 删除单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Entity"></param>
        /// <returns></returns>
        bool Delete<T>(T Entity) where T : class;

        /// <summary>
        /// 根据查询条件进行删除单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        bool Delete<T>(Expression<Func<T, bool>> whereLambda) where T : class;


        /// <summary>
        ///单个对象的修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Entity">需要修改的对象</param>
        /// <returns></returns>
        bool Update<T>(T Entity) where T : class;


        /// <summary>
        /// 批量修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLambda"></param>
        /// <param name="updateLambda"></param>
        /// <returns></returns>
        //bool Update<T>(Expression<Func<T, bool>> WhereLambda, Expression<Func<T, T>> UpdateLambda) where T : class;


        /// <summary>
        /// 批量的修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Entity"></param>
        /// <returns></returns>
        bool Update<T>(List<T> Entity) where T : class;


        /// <summary>
        /// 批量统一的进行更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">需要修改的对象实体</param>
        /// <param name="WhereLambda">查询的条件</param>
        /// <param name="ModifiedProNames"></param>
        /// <returns></returns>
        bool Update<T>(T model, Expression<Func<T, bool>> WhereLambda, params string[] ModifiedProNames) where T : class;


        /// <summary>
        /// 根据主键进行查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ID"></param>
        /// <returns></returns>
        T FindByID<T>(dynamic ID) where T : class;

        /// <summary>
        /// 默认查询选择第一条数据,没有那么进行返回NULL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="WhereLambda"></param>
        /// <returns>返回bool</returns>
        T GetFristDefault<T>(Expression<Func<T, bool>> WhereLambda = null) where T : class;

        /// <summary>
        /// 查询所有的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        //List<T> GetAll<T>(string Order = null) where T : class;

        /// <summary>
        /// 含有带条件的查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="WhereLambda"></param>
        /// <returns></returns>
        List<T> GetAllQuery<T>(Expression<Func<T, bool>> WhereLambda = null) where T : class;


        /// <summary>
        ///获取查询的数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="WhereLambda"></param>
        /// <returns></returns>
        int GetCount<T>(Expression<Func<T, bool>> WhereLambda = null) where T : class;

        /// <summary>
        /// 判断对象是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="WhereLambda"></param>
        /// <returns></returns>
        bool GetAny<T>(Expression<Func<T, bool>> WhereLambda = null) where T : class;


        /// <summary>
        /// 根据查询过条件进行分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="PageIndex">当前页面</param>
        /// <param name="PageSize">页面的大小</param>
        /// <param name="TotalCount">总记录数</param>
        /// <param name="OrderBy">排序的条件</param>
        /// <param name="WhereLambda">查询条件</param>
        /// <param name="IsOrder">是否正序</param>
        /// <returns></returns>
        List<T> Pagination<T, TKey>(int PageIndex, int PageSize, out int TotalCount, Expression<Func<T, TKey>> OrderBy, Expression<Func<T, bool>> WhereLambda = null, bool IsOrder = true) where T : class;


        /// <summary>
        /// 根据查询条件进行做分页查询
        /// </summary>
        /// <typeparam name="T">查询的对象</typeparam>
        /// <param name="PageIndex">当前的页码</param>
        /// <param name="PageSize">每页的大小</param>
        /// <param name="TotalCount">总页数</param>
        /// <param name="ordering">排序条件</param>
        /// <param name="WhereLambda">查询条件</param>
        /// <returns></returns>
        //List<T> Pagination<T>(int PageIndex, int PageSize, out int TotalCount, string ordering, Expression<Func<T, bool>> WhereLambda = null) where T : class;


        /// <summary>
        /// 根据查询条件进行转化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="WhereLambda"></param>
        /// <returns></returns>
        List<T> GetSelect<T>(Expression<Func<T, bool>> WhereLambda) where T : class;

        /// <summary>
        /// 回滚
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void RollBackChanges<T>() where T : class;

    }
}
