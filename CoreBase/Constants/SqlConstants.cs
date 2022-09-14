using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreBase.Constants
{
    public static class SqlConstants
    {

        #region Constants

        public const int DefaultPageSize = 50;
        public const int MinimumPageSize = 50;

        public const string BetweenInclusiveLiteral = "is between inclusive";
        public const string BetweenExclusiveLiteral = "is between exclusive";
        public const string SelectIdentity = "SELECT @@IDENTITY";
        public const string PARAM_MARKER = "@";
        public const string QUESTION_MARK = "?";

        public const string TRUE_COMPARE = "TRUE_COMPARE";
        public const string FALSE_COMPARE = "FALSE_COMPARE";

        #endregion

        #region SQL Query Parts

        public const string NEW_LINE = "\n";
        public const string EQUALS = "=";
        public const string DOT = ".";

        public const string WILD_CARD = "*";
        public const string SINGLE_QUOTE = "'";
        public const string DOUBLE_QUOTE = "\"";
        public const string POUND = "#";
        public const string PER_CENT = "%";
        public const string USE = "use";
        public const string SELECT = "select";
        public const string AS = "as";
        public const string COMMA = ",";
        public const string FROM = "from";
        public const string WHERE = "where";
        public const string AND = "and";
        public const string OR = "or";
        public const string STATEMENT_DELIMITER = ";";
        public const string APOSTROPHE = "'";
        public const string ASTERISK = "*";
        public const string PERIOD = ".";
        public const string LEFT_PAREN = "(";
        public const string RIGHT_PAREN = ")";
        public const string LEFT_SQUARE = "[";
        public const string RIGHT_SQUARE = "]";
        public const string SPACE = " ";
        public const string Space = " ";
        public const string CR = @"\r";
        public const string LF = @"\n";
        public const string TAB = @"\t";
        public const string DISTINCT = "distinct";
        public const string DISTINCTROW = "distinctrow";

        public const string EQUAL = "=";
        public const string GREATER_THAN = ">";
        public const string GREATER_THAN_EQUAL = ">=";
        public const string LESS_THAN = "<";
        public const string LESS_THAN_EQUAL = "<=";
        public const string NOT_EQUAL = "<>";
        public const string IS = "is";
        public const string IS_NOT = "is not";
        public const string LIKE = "like";

        public const string CONTAINS = "contains";
        public const string BEGINS_WITH = "begins with";
        public const string ENDS_WITH = "ends with";

        public const string DOESNT_CONTAIN = "doesn\'t contain";
        public const string DOESNT_BEGIN_WITH = "doesn\'t begin with";
        public const string DOESNT_END_WITH = "doesn\'t end with";

        public const string TERMINATOR = ";";
        public const string ORDER_BY = "order by";
        public const string JOIN = "join";
        public const string LEFT_JOIN = "left join";
        public const string RIGHT_JOIN = "right join";
        public const string INNER_JOIN = "inner join";
        public const string OUTER_JOIN = "outer join";

        public const string GROUP_BY = "group by";
        public const string UNION = "union";
        public const string HAVING = "having";

        public const string ON = "on";
        public const string OFF = "off";

        public const string SELECT_DISTINCT = SqlConstants.SELECT + SqlConstants.SPACE + SqlConstants.DISTINCT;

        #endregion

        #region SQL Schema

        public const string IS_AUTO_INCREMENT = "IsAutoIncrement";
        public const string DATA_TYPE = "DataType";
        public const string COLUMN_NAME = "ColumnName";
        public const string BASE_COLUMN_NAME = "BaseTableName";
        public const string BASE_TABLE_NAME = "BaseTableName";
        public const string TABLE_NAME = "TABLE_NAME";
        public const string TABLE_TYPE = "TABLE_Type";
        public const string TABLE = "TABLE";
        public const string TABLES = "Tables";
        public const string TABLE_SCHEMA = "TABLE_SCHEMA";

        #endregion

        #region SQL Methods

        public const string DATEDIFF = "DATEDIFF";
        public const string COUNT = "COUNT(*)";


        #endregion

        #region SQL Parameter Start/End

        public const string PARAM_START = "{";
        public const string PARAM_END = "}";

        #endregion

        #region SQL Schema Queries

        /// <summary>
        /// Empty query that retrieves column metadata but no results
        /// </summary>
        
        public const string GET_SCHEMA_QUERY =  SELECT + " * " + FROM + " {0} where 1=0";
        public const string GET_SCHEMA_STATEMENT_AND_APPEND = " and 1=0";
        public const string GET_SCHEMA_STATEMENT_WHERE_APPEND = " " + WHERE + " 1=0";

        #endregion

        #region DateTime Contstants

        public const string DateTimeEndOfDay = "23:59:59";

        #endregion

        #region Gt SQL Constants

        public const string PARAM_SCHEMA_NAME = "<%SchemaName%>";
        public const string SchemaNameScript = "SchemaName";
        public const string DatabaseNameScript = "DatabaseName";
        public const string FilePathScript = "FilePath";
        public const string LoginNameScript = "LoginName";
        public const string UserNameScript = "UserName";
        public const string PasswordScript = "Password";
        public const string SchemaPlaceholderScript = "SchemaPlaceholder";

        #endregion

        #region Properties

        static string[] _columnMarkers = { SqlConstants.COMMA, SqlConstants.LEFT_PAREN, SqlConstants.RIGHT_PAREN };
        public static string[] ColumnMarkers
        { get { return _columnMarkers; } }

        static string[] _asMarkers = { SqlConstants.AS };
        public static string[] AliasMarkers
        { get { return _asMarkers; } }

        static string[] _spaceMarkers = { SqlConstants.SPACE, SqlConstants.TAB };
        public static string[] SpaceMarkers
        { get { return _spaceMarkers; } }

        static string[] _dotMarkers = { SqlConstants.PERIOD, SqlConstants.SPACE };
        public static string[] DotMarkers
        { get { return _dotMarkers; } }


        #endregion



    }
}
