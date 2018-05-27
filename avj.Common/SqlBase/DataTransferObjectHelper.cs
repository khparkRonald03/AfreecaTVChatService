using System;
using System.Collections;
using System.Data;
using System.Reflection;

namespace avj.Common
{
    public static class DataTransferObjectHelper
    {
        /// <summary>
        /// DataTable 객체를 ToDataObject의 메소드를 통해 Data Object 형태로 변환해주는 메서드
        /// DataTable 객체에 확장 메서드로 등록되어 사용 됩니다.
        /// </summary>
        /// <param name="datatable"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ToDataObject(this DataTable datatable, Type type)
        {
            object dto = Activator.CreateInstance(type);
            string fieldName = string.Empty;

            if (dto is IList)
            {
                Type t = dto.GetType().GetGenericArguments()[0];
                //여러개의 ROW 가 있을 경우
                foreach (DataRow dr in datatable.Rows)
                {
                    object listItem = Activator.CreateInstance(t);

                    foreach (DataColumn dc in datatable.Columns)
                    {
                        fieldName = dc.ColumnName.ToString();

                        //PropertyInfo p = t.GetProperty(fieldName) == null ? t.GetProperty(string.Format("{0}.{1}", t.Name, fieldName)) : t.GetProperty(fieldName);
                        PropertyInfo p = t.GetProperty(fieldName);

                        if (p != null)
                        {
                            if (dr.RowState == DataRowState.Deleted)
                            {
                                p.SetValue(listItem, dr[dc, DataRowVersion.Original], null);
                            }
                            else
                            {
                                p.SetValue(listItem, dr[dc] == DBNull.Value ? null : dr[dc], null);
                            }
                        }
                        //if (listItem.GetType().GetProperties().Where(p => p.Name == dc.ColumnName).Count() > 0)
                        //{
                        //    if (dr.RowState == DataRowState.Deleted)
                        //    {
                        //        listItem.GetType().GetProperty(dc.ColumnName).SetValue(listItem, dr[dc, DataRowVersion.Original], null);
                        //    }
                        //    else
                        //    {
                        //        listItem.GetType().GetProperty(dc.ColumnName).SetValue(listItem, dr[dc] == DBNull.Value ? null : dr[dc], null);
                        //    }

                        //}
                    }
                    (dto as IList).Add(listItem);
                }
            }
            else
            {
                if (datatable.Rows.Count > 0)
                {
                    //단건의 ROW 처리
                    foreach (var property in dto.GetType().GetProperties())
                    {
                        string pname = string.Empty;
                        if (datatable.Columns.Contains(property.Name) == true)
                        {
                            pname = property.Name;
                        }
                        //else if (datatable.Columns.Contains(string.Format("{0}.{1}", dto.GetType().Name, property.Name)) == true)
                        //{
                        //    pname = string.Format("{0}.{1}", dto.GetType().Name, property.Name);
                        //}

                        if (pname.Length > 0)
                        {
                            if (datatable.Rows[0].RowState == DataRowState.Deleted)
                            {
                                property.SetValue(dto, datatable.Rows[0][pname, DataRowVersion.Original], null);
                            }
                            else
                            {
                                property.SetValue(dto, datatable.Rows[0][property.Name] == DBNull.Value ? null : datatable.Rows[0][pname], null);
                            }
                        }
                    }
                }
            }

            return dto;
        }

    }
}
