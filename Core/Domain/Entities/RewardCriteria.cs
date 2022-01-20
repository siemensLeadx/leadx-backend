using Helpers.Constants;

namespace Domain.Entities
{
    public class RewardCriteria
    {
        public int Id { get; set; }
        public decimal? From { get; set; }
        public decimal? To { get; set; }

        public string ToCustomStringFormat(string currentLang)
        {
            var leadValueText = currentLang.Contains(KeyValueConstants.Arabic) ?
                "قيمة المبادرة" : "Lead value";

            var oneMillion = 1000000;

            if (!From.HasValue)
                return $"{leadValueText} < {(int)(To / oneMillion)}M";
            else if (!To.HasValue)
                return $"{leadValueText} >= {(int)(From / oneMillion)}M";
            else
                return $"{(int)(From / oneMillion)}M <= {leadValueText} < {(int)(To / oneMillion)}M";
        }
    }
}
