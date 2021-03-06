using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CadeOFogo.Migrations
{
    public partial class inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NomeCompleto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FotoPerfil = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Batalhoes",
                columns: table => new
                {
                    BatalhaoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeBatalhao = table.Column<string>(type: "nvarchar(280)", maxLength: 280, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Batalhoes", x => x.BatalhaoId);
                });

            migrationBuilder.CreateTable(
                name: "CausadoresProvaveis",
                columns: table => new
                {
                    CausadorProvavelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CausadorProvavelDescricacao = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CausadoresProvaveis", x => x.CausadorProvavelId);
                });

            migrationBuilder.CreateTable(
                name: "CausasFogo",
                columns: table => new
                {
                    CausaFogoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CausaFogoDescricao = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CausasFogo", x => x.CausaFogoId);
                });

            migrationBuilder.CreateTable(
                name: "Estados",
                columns: table => new
                {
                    EstadoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstadoNome = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    EstadoIdInpe = table.Column<int>(type: "int", nullable: false),
                    UltimoFocoObservadoUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Monitorado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estados", x => x.EstadoId);
                });

            migrationBuilder.CreateTable(
                name: "IndiciosInicioFoco",
                columns: table => new
                {
                    IndicioInicioFocoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IndicioInicioFocoDescricao = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndiciosInicioFoco", x => x.IndicioInicioFocoId);
                });

            migrationBuilder.CreateTable(
                name: "ResponsaveisPropriedade",
                columns: table => new
                {
                    ResponsavelPropriedadeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResponsavelPropriedadeDescricao = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponsaveisPropriedade", x => x.ResponsavelPropriedadeId);
                });

            migrationBuilder.CreateTable(
                name: "Satelites",
                columns: table => new
                {
                    SateliteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SateliteNome = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    SateliteNomeINPE = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    UltimoFocoUtc = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    Monitorado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Satelites", x => x.SateliteId);
                });

            migrationBuilder.CreateTable(
                name: "StatusFocos",
                columns: table => new
                {
                    StatusFocoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusFocoDescricao = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusFocos", x => x.StatusFocoId);
                });

            migrationBuilder.CreateTable(
                name: "TiposVegetacao",
                columns: table => new
                {
                    TipoVegetacaoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoVegetacaoDescricao = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposVegetacao", x => x.TipoVegetacaoId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Companhias",
                columns: table => new
                {
                    CompanhiaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanhiaNome = table.Column<string>(type: "nvarchar(280)", maxLength: 280, nullable: false),
                    BatalhaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companhias", x => x.CompanhiaId);
                    table.ForeignKey(
                        name: "FK_Companhias_Batalhoes_BatalhaoId",
                        column: x => x.BatalhaoId,
                        principalTable: "Batalhoes",
                        principalColumn: "BatalhaoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Municipios",
                columns: table => new
                {
                    MunicipioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MunicioIbgeId = table.Column<int>(type: "int", nullable: false),
                    MunicipioNome = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    EstadoId = table.Column<int>(type: "int", nullable: false),
                    Monitorado = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    UltimoFocoObservadoUtc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipios", x => x.MunicipioId);
                    table.UniqueConstraint("AK_Municipios_MunicioIbgeId", x => x.MunicioIbgeId);
                    table.ForeignKey(
                        name: "FK_Municipios_Estados_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "Estados",
                        principalColumn: "EstadoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pelotoes",
                columns: table => new
                {
                    PelotaoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PelotaoNome = table.Column<string>(type: "nvarchar(280)", maxLength: 280, nullable: false),
                    CompanhiaId = table.Column<int>(type: "int", nullable: false),
                    BatalhaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pelotoes", x => x.PelotaoId);
                    table.ForeignKey(
                        name: "FK_Pelotoes_Batalhoes_BatalhaoId",
                        column: x => x.BatalhaoId,
                        principalTable: "Batalhoes",
                        principalColumn: "BatalhaoId");
                    table.ForeignKey(
                        name: "FK_Pelotoes_Companhias_CompanhiaId",
                        column: x => x.CompanhiaId,
                        principalTable: "Companhias",
                        principalColumn: "CompanhiaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Equipes",
                columns: table => new
                {
                    EquipeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipeNome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BatalhaoId = table.Column<int>(type: "int", nullable: false),
                    CompanhiaId = table.Column<int>(type: "int", nullable: false),
                    PelotaoId = table.Column<int>(type: "int", nullable: false),
                    Ativa = table.Column<bool>(type: "bit", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipes", x => x.EquipeId);
                    table.ForeignKey(
                        name: "FK_Equipes_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Equipes_Batalhoes_BatalhaoId",
                        column: x => x.BatalhaoId,
                        principalTable: "Batalhoes",
                        principalColumn: "BatalhaoId");
                    table.ForeignKey(
                        name: "FK_Equipes_Companhias_CompanhiaId",
                        column: x => x.CompanhiaId,
                        principalTable: "Companhias",
                        principalColumn: "CompanhiaId");
                    table.ForeignKey(
                        name: "FK_Equipes_Pelotoes_PelotaoId",
                        column: x => x.PelotaoId,
                        principalTable: "Pelotoes",
                        principalColumn: "PelotaoId");
                });

            migrationBuilder.CreateTable(
                name: "UsuarioPelotao",
                columns: table => new
                {
                    UsuarioPeltao_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pelotao_Id = table.Column<int>(type: "int", nullable: false),
                    User_Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioPelotao", x => x.UsuarioPeltao_Id);
                    table.UniqueConstraint("AK_UsuarioPelotao_Pelotao_Id_User_Id", x => new { x.Pelotao_Id, x.User_Id });
                    table.ForeignKey(
                        name: "FK_UsuarioPelotao_AspNetUsers_User_Id",
                        column: x => x.User_Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UsuarioPelotao_Pelotoes_Pelotao_Id",
                        column: x => x.Pelotao_Id,
                        principalTable: "Pelotoes",
                        principalColumn: "PelotaoId");
                });

            migrationBuilder.CreateTable(
                name: "Focos",
                columns: table => new
                {
                    FocoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FocoLongitude = table.Column<decimal>(type: "decimal(13,8)", precision: 13, scale: 8, nullable: false),
                    FocoLatitude = table.Column<decimal>(type: "decimal(13,8)", precision: 13, scale: 8, nullable: false),
                    FocoDataUtc = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    FocoAtendido = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    FocoConfirmado = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SateliteId = table.Column<int>(type: "int", nullable: false),
                    MunicipioId = table.Column<int>(type: "int", nullable: false),
                    EstadoId = table.Column<int>(type: "int", nullable: false),
                    SnapshotSatelite = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    DataSnapshot = table.Column<DateTime>(type: "datetime2(0)", precision: 0, nullable: false),
                    SnapshotProvider = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    InpeFocoId = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Bioma = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Municipi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PolicialResponsavel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OcorrênciaSIOPM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NºBOPAmb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NºTVA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RSO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataAtendimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EquipeId = table.Column<int>(type: "int", nullable: false),
                    StatusFocoId = table.Column<int>(type: "int", nullable: false),
                    IndicioInicioFocoId = table.Column<int>(type: "int", nullable: false),
                    CausaFogoId = table.Column<int>(type: "int", nullable: false),
                    CausadorProvavelId = table.Column<int>(type: "int", nullable: false),
                    ResponsavelPropriedadeId = table.Column<int>(type: "int", nullable: false),
                    TipoVegetacaoId = table.Column<int>(type: "int", nullable: false),
                    PioneiroAPPAreaEmHectares = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InicialAPPAreaEmHectares = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedioAPPAreaEmHectares = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvancadoAPPAreaEmHectares = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AutoDeInflacaoAmbientalAPP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MultaAPP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pioneiro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Inicial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Medio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Avancado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AutoDeInflacaoAmbiental = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MultaR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pasto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Citrus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Outras = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AutoDeInflacaoAmbientalV = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MultaV = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArvoresIsoladas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AutoDeInflacaoAmbientalA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MultaA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PalhaDeCana = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CanaDeAcucar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Autorizado = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AutoDeInflacaoAmbientalL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MultaL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PioneiroUC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InicialUC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedioUC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvancadoUC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OutrasUC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AutoDeInflacaoAmbientalUC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MultaUC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PioneiroRL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InicialRL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedioRL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvancadoRL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OutrasRL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AutoDeInflacaoAmbientalRL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MultaRL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Refiscalizacao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Focos", x => x.FocoId);
                    table.UniqueConstraint("AK_Focos_FocoLatitude_FocoLongitude_FocoDataUtc_SateliteId", x => new { x.FocoLatitude, x.FocoLongitude, x.FocoDataUtc, x.SateliteId });
                    table.ForeignKey(
                        name: "FK_Focos_CausadoresProvaveis_CausadorProvavelId",
                        column: x => x.CausadorProvavelId,
                        principalTable: "CausadoresProvaveis",
                        principalColumn: "CausadorProvavelId");
                    table.ForeignKey(
                        name: "FK_Focos_CausasFogo_CausaFogoId",
                        column: x => x.CausaFogoId,
                        principalTable: "CausasFogo",
                        principalColumn: "CausaFogoId");
                    table.ForeignKey(
                        name: "FK_Focos_Equipes_EquipeId",
                        column: x => x.EquipeId,
                        principalTable: "Equipes",
                        principalColumn: "EquipeId");
                    table.ForeignKey(
                        name: "FK_Focos_Estados_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "Estados",
                        principalColumn: "EstadoId");
                    table.ForeignKey(
                        name: "FK_Focos_IndiciosInicioFoco_IndicioInicioFocoId",
                        column: x => x.IndicioInicioFocoId,
                        principalTable: "IndiciosInicioFoco",
                        principalColumn: "IndicioInicioFocoId");
                    table.ForeignKey(
                        name: "FK_Focos_Municipios_MunicipioId",
                        column: x => x.MunicipioId,
                        principalTable: "Municipios",
                        principalColumn: "MunicipioId");
                    table.ForeignKey(
                        name: "FK_Focos_ResponsaveisPropriedade_ResponsavelPropriedadeId",
                        column: x => x.ResponsavelPropriedadeId,
                        principalTable: "ResponsaveisPropriedade",
                        principalColumn: "ResponsavelPropriedadeId");
                    table.ForeignKey(
                        name: "FK_Focos_Satelites_SateliteId",
                        column: x => x.SateliteId,
                        principalTable: "Satelites",
                        principalColumn: "SateliteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Focos_StatusFocos_StatusFocoId",
                        column: x => x.StatusFocoId,
                        principalTable: "StatusFocos",
                        principalColumn: "StatusFocoId");
                    table.ForeignKey(
                        name: "FK_Focos_TiposVegetacao_TipoVegetacaoId",
                        column: x => x.TipoVegetacaoId,
                        principalTable: "TiposVegetacao",
                        principalColumn: "TipoVegetacaoId");
                });

            migrationBuilder.InsertData(
                table: "Batalhoes",
                columns: new[] { "BatalhaoId", "NomeBatalhao" },
                values: new object[] { 1, "4º Batalhão da Polícia Ambiental" });

            migrationBuilder.InsertData(
                table: "CausadoresProvaveis",
                columns: new[] { "CausadorProvavelId", "CausadorProvavelDescricacao" },
                values: new object[,]
                {
                    { 3, "Outro Identificado" },
                    { 2, "Proprietário" },
                    { 1, "Funcionário" },
                    { 4, "Indeterminado" }
                });

            migrationBuilder.InsertData(
                table: "CausasFogo",
                columns: new[] { "CausaFogoId", "CausaFogoDescricao" },
                values: new object[,]
                {
                    { 11, "Terreno Baldio" },
                    { 10, "Queima Ponto de Apoio" },
                    { 9, "Incidente com colheitadeiras" },
                    { 8, "Limpeza de controle fitossanitário" },
                    { 6, "Raio/Causas naturais" },
                    { 5, "Vandalismo" },
                    { 4, "Queima de resto de cultura" },
                    { 3, "Queima para Limpeza" },
                    { 2, "Queima de Cana" },
                    { 1, "Desconhecida" },
                    { 7, "Descarga elétrica" }
                });

            migrationBuilder.InsertData(
                table: "IndiciosInicioFoco",
                columns: new[] { "IndicioInicioFocoId", "IndicioInicioFocoDescricao" },
                values: new object[,]
                {
                    { 3, "Indeterminado" },
                    { 2, "Rodovia/Estrada" },
                    { 1, "Dentro da Propriedade" }
                });

            migrationBuilder.InsertData(
                table: "ResponsaveisPropriedade",
                columns: new[] { "ResponsavelPropriedadeId", "ResponsavelPropriedadeDescricao" },
                values: new object[,]
                {
                    { 1, "Não Informado no TVA/BO" },
                    { 2, "Proprietário Fornecedor" },
                    { 3, "Usina Arrendatário/Proprietária" },
                    { 4, "Empresa Arrendatário" },
                    { 5, "Prefeitura" },
                    { 6, "Proprietário (Não Usina)" }
                });

            migrationBuilder.InsertData(
                table: "StatusFocos",
                columns: new[] { "StatusFocoId", "StatusFocoDescricao" },
                values: new object[,]
                {
                    { 5, "Autorizado desacordo" },
                    { 4, "Autorizado" },
                    { 2, "Sem nexo causalidade" },
                    { 1, "Não encontrado" },
                    { 3, "Não autorizado" }
                });

            migrationBuilder.InsertData(
                table: "TiposVegetacao",
                columns: new[] { "TipoVegetacaoId", "TipoVegetacaoDescricao" },
                values: new object[,]
                {
                    { 5, "Vegetação nativa primária" },
                    { 1, "Vegetação pioneira ou demais formas de vegetação natural" },
                    { 2, "Vegetação nativa secundária em estágio inicial de regeneração" },
                    { 3, "Vegetação nativa secundária em estágio médio de regeneração" },
                    { 4, "Vegetação nativa secundária em estágio avançado de regeneração" },
                    { 6, "Vegetação exótica" }
                });

            migrationBuilder.InsertData(
                table: "Companhias",
                columns: new[] { "CompanhiaId", "BatalhaoId", "CompanhiaNome" },
                values: new object[] { 1, 1, "1º Companhia da Polícia Ambiental" });

            migrationBuilder.InsertData(
                table: "Pelotoes",
                columns: new[] { "PelotaoId", "BatalhaoId", "CompanhiaId", "PelotaoNome" },
                values: new object[] { 1, 1, 1, "1º Pelotão da Polícia Ambiental" });

            migrationBuilder.InsertData(
                table: "Equipes",
                columns: new[] { "EquipeId", "ApplicationUserId", "Ativa", "BatalhaoId", "CompanhiaId", "EquipeNome", "PelotaoId" },
                values: new object[] { 1, null, true, 1, 1, "Sem Equipe", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Companhias_BatalhaoId",
                table: "Companhias",
                column: "BatalhaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipes_ApplicationUserId",
                table: "Equipes",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipes_BatalhaoId",
                table: "Equipes",
                column: "BatalhaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipes_CompanhiaId",
                table: "Equipes",
                column: "CompanhiaId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipes_PelotaoId",
                table: "Equipes",
                column: "PelotaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Focos_CausadorProvavelId",
                table: "Focos",
                column: "CausadorProvavelId");

            migrationBuilder.CreateIndex(
                name: "IX_Focos_CausaFogoId",
                table: "Focos",
                column: "CausaFogoId");

            migrationBuilder.CreateIndex(
                name: "IX_Focos_EquipeId",
                table: "Focos",
                column: "EquipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Focos_EstadoId",
                table: "Focos",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Focos_IndicioInicioFocoId",
                table: "Focos",
                column: "IndicioInicioFocoId");

            migrationBuilder.CreateIndex(
                name: "IX_Focos_MunicipioId",
                table: "Focos",
                column: "MunicipioId");

            migrationBuilder.CreateIndex(
                name: "IX_Focos_ResponsavelPropriedadeId",
                table: "Focos",
                column: "ResponsavelPropriedadeId");

            migrationBuilder.CreateIndex(
                name: "IX_Focos_SateliteId",
                table: "Focos",
                column: "SateliteId");

            migrationBuilder.CreateIndex(
                name: "IX_Focos_StatusFocoId",
                table: "Focos",
                column: "StatusFocoId");

            migrationBuilder.CreateIndex(
                name: "IX_Focos_TipoVegetacaoId",
                table: "Focos",
                column: "TipoVegetacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Municipios_EstadoId",
                table: "Municipios",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pelotoes_BatalhaoId",
                table: "Pelotoes",
                column: "BatalhaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pelotoes_CompanhiaId",
                table: "Pelotoes",
                column: "CompanhiaId");

            migrationBuilder.CreateIndex(
                name: "IX_Satelites_SateliteNomeINPE",
                table: "Satelites",
                column: "SateliteNomeINPE");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioPelotao_User_Id",
                table: "UsuarioPelotao",
                column: "User_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Focos");

            migrationBuilder.DropTable(
                name: "UsuarioPelotao");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "CausadoresProvaveis");

            migrationBuilder.DropTable(
                name: "CausasFogo");

            migrationBuilder.DropTable(
                name: "Equipes");

            migrationBuilder.DropTable(
                name: "IndiciosInicioFoco");

            migrationBuilder.DropTable(
                name: "Municipios");

            migrationBuilder.DropTable(
                name: "ResponsaveisPropriedade");

            migrationBuilder.DropTable(
                name: "Satelites");

            migrationBuilder.DropTable(
                name: "StatusFocos");

            migrationBuilder.DropTable(
                name: "TiposVegetacao");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Pelotoes");

            migrationBuilder.DropTable(
                name: "Estados");

            migrationBuilder.DropTable(
                name: "Companhias");

            migrationBuilder.DropTable(
                name: "Batalhoes");
        }
    }
}
