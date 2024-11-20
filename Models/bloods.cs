﻿namespace BloodBank.Models
{
    //models provided below
    public class bloods
    {
        public int Id { get; set; } 
        public string? DonorName { get; set; }
        public int age { get; set; }
        public string? BloodType { get; set; }
        public string? ContactInfo { get; set; }
        public int Quantity { get; set; }
        public DateTime CollectionDate { get; set; }
        public DateTime ExpiratoinDate  { get; set; }
        public string? Status { get; set; }
    }
}