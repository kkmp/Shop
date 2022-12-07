﻿namespace Shop.DTO.Product
{
    public class ProductGetDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public string? Photo { get; set; }
        public string? Price { get; set; }
    }
}
