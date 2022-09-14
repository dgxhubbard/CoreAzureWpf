using System;
using System.Collections.Generic;
using System.Text;


using CoreBase;
using CoreBase.Enums;
using CoreBase.Security;



namespace CoreModel.Interfaces
{
    public interface IGatewayBase<T>
        where T : BaseModel
    {
        void Add ( T item, TargetDatabase target, SecurityToken securityToken );

        List<string> GetProperty ( string propertyName, TargetDatabase target, SecurityToken securityToken );

        List<T> Get ( TargetDatabase target, SecurityToken securityToken );

        List<T> GetWithoutNav ( TargetDatabase target, SecurityToken securityToken );

        T GetByRid ( int itemRid, TargetDatabase target, SecurityToken securityToken );

        List<T> GetByRids ( List<int> itemRid, TargetDatabase target, SecurityToken securityToken );

        int GetCount ( TargetDatabase target, SecurityToken securityToken );

        List<T> GetPaged ( int offset, int pageSize, TargetDatabase target, SecurityToken securityToken );

        List<T> GetPagedWithoutNav ( int offset, int pageSize, TargetDatabase target, SecurityToken securityToken );

        List<T> GetFilterWithoutNav ( string filterExpression, string sortExpression, TargetDatabase target, SecurityToken securityToken );

        void Remove ( int itemRid, TargetDatabase target, SecurityToken securityToken );

        void Remove ( List<int> itemRids, TargetDatabase target, SecurityToken securityToken );

        int Update ( T item, TargetDatabase target, SecurityToken securityToken );

        List<int> Update ( List<T> items, TargetDatabase target, SecurityToken securityToken );


    }
}
