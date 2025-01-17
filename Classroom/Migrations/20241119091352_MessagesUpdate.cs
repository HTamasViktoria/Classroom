﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Classroom.Migrations
{
    /// <inheritdoc />
    public partial class MessagesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HeadText",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeadText",
                table: "Messages");
        }
    }
}
