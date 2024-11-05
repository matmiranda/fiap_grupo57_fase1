using fiap_grupo57_fase1.Interfaces.Models.Utils;

namespace fiap_grupo57_fase1.Models.Utils
{
    public class ObterRegiaoPorDDD : IObterRegiaoPorDDD
    {
        public string ObtemRegiaoPorDDD(int DDD)
        {
            string regiao = string.Empty;
            switch (DDD)
            {
                case 63:
                case 68:
                case 69:
                case 92:
                case 95:
                case 96:
                case 97:
                    regiao = "1";
                    break;
                case 71:
                case 73:
                case 74:
                case 75:
                case 77:
                case 79:
                case 81:
                case 82:
                case 83:
                case 84:
                case 85:
                case 86:
                case 87:
                case 88:
                case 89:
                case 98:
                case 99:
                    regiao = "2";
                    break;
                case 61:
                case 62:
                case 64:
                case 65:
                case 66:
                case 67:
                    regiao = "3";
                    break;
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 21:
                case 22:
                case 24:
                case 27:
                case 28:
                case 31:
                case 32:
                case 33:
                case 34:
                case 35:
                case 37:
                case 38:
                    regiao = "4";
                    break;
                case 41:
                case 42:
                case 43:
                case 44:
                case 45:
                case 46:
                case 47:
                case 48:
                case 49:
                case 51:
                case 53:
                case 54:
                case 55:
                    regiao = "5";
                    break;
                default:
                    regiao = $"DDD_INVALIDO";
                    break;
            }
            return regiao;
        }
    }
}
