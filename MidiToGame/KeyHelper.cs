namespace MidiToGame
{
    public static class KeyHelper
    {
        public static byte GetKeyByte(string keyName)
        {
            try
            {
                if (Enum.TryParse(typeof(Keys), keyName, true, out object? result) && result is Keys key)
                {
                    return (byte)key;
                }
                throw new Exception($"Invalid Key [{keyName}]");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                throw;
            }

        }
    }
}
