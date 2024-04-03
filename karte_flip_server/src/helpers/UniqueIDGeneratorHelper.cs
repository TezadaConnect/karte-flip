using System;

public class UniqueIDGeneratorHelper{
    // Function to generate a unique ID
    public static string GenerateUniqueID()
    {
        // Get current timestamp
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        // Generate a random number
        Random random = new Random();
        int randomNumber = random.Next(1000, 9999); // Example range, adjust as needed

        // Combine timestamp and random number to create a unique ID
        string uniqueID = $"{timestamp}-{randomNumber}";

        return uniqueID;
    }
}