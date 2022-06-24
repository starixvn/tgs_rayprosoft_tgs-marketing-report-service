using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tgs.MarketingReport.Core;

public record MarketingReportDailyAggregateInfo(
    Guid AgentId,
    string AgentUserName,
    DateOnly ReportDate,
    int RegisterCount,
    int FirstDepositCount,
    int GamingCount);