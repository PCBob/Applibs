
using System;
using System.Collections.Generic;

namespace Applibs.Where
{
    public class WhereClauseResult
    {
        internal WhereClauseResult(string whereClause, IDictionary<string, object> parameters)
        {
            if (string.IsNullOrEmpty(whereClause))
            {
                throw new ArgumentNullException(nameof(whereClause));
            }
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            this.WhereClause = whereClause;
            this.Parameter = new Dictionary<string, object>().Copy(parameters);
        }

        public string WhereClause { get; }

        public IDictionary<string, object> Parameter { get; }
    }
}