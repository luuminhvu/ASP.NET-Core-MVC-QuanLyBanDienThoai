using System.Text.RegularExpressions;

namespace QuanLyBanDienThoai.Extension
{
    public static class Extension
    {
        public static string ToVND(this double donGia)
        {
            return donGia.ToString("#,##0") + " đ";
        }
        public static string ToTitleCase(String str)
        {
            string result = str;
            if (!string.IsNullOrEmpty(str))
            {
                var words = str.Split(' ');
                for (int index = 0; index < words.Length; index++)
                {
                    var s = words[index];
                    if (s.Length > 0)
                    {
                        words[index] = s[0].ToString().ToUpper() + s.Substring(1);
                    }
                }
                result = string.Join(" ", words);
            }
            return result;
        }
        public static string ToUrlFriendly(this string url)
        {
            // Xóa khoảng trắng đầu và cuối chuỗi
            var result = url.ToLower().Trim();

            // Chuyển các ký tự có dấu sang không dấu
            result = Regex.Replace(result, "[àáảạãăắằẳẵặâầấẩẫậ]", "a");
            result = Regex.Replace(result, "[èéẻẹẽêềếểễệ]", "e");
            result = Regex.Replace(result, "[đ]", "d");
            result = Regex.Replace(result, "[ìíỉịĩ]", "i");
            result = Regex.Replace(result, "[òóỏọõôồốổỗộơờớởỡợ]", "o");
            result = Regex.Replace(result, "[ùúủụũưừứửữự]", "u");
            result = Regex.Replace(result, "[ỳýỷỵỹ]", "y");

            // Xóa các ký tự đặc biệt không cần thiết
            result = Regex.Replace(result, "[^a-z0-9]", " ");

            // Xóa các dấu gạch ngang thừa
            result = Regex.Replace(result, "-+", "-");

            // Xóa ký tự gạch ngang ở đầu và cuối chuỗi
            result = result.Trim('-');

            // Xóa khoảng trắng và chuyển ký tự đầu tiên của mỗi từ thành chữ hoa
            result = string.Join("", result.Split(' ').Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1)));

            return result;
        }

    }
}
