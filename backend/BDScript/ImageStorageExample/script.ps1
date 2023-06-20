clear
"Rode o script do banco de dados antes de dar ENTER e seguir. Também tenha certeza que não terá problemas com o Proxy do Nuget."
Read-Host

mkdir FullExample
cd FullExample

mkdir Model
cd Model
dotnet new classlib
rm Class1.cs
"DataSource:"
$dataSource = Read-Host
$initialCatalog = "FullExample"
$strconn = "Data Source=" + $dataSource + ";Initial Catalog=" + $initialCatalog + ";Integrated Security=True;TrustServerCertificate=true"
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet tool install --global dotnet-ef
dotnet ef dbcontext scaffold $strconn Microsoft.EntityFrameworkCore.SqlServer --force
cd ..

mkdir DTO
cd DTO
dotnet new classlib
rm Class1.cs
ni LocationDTO.cs
sc LocationDTO.cs "namespace DTO;

public class LocationDTO
{
    public string Name { get; set; }
    public string ImgPath { get; set; }
}"
cd ..

mkdir Back
cd Back
dotnet new webapi
dotnet add reference ../Model/Model.csproj
dotnet add reference ../DTO/DTO.csproj
rm WeatherForecast.cs
cd Controllers
rm WeatherForecastController.cs
ni ImageController.cs
sc ImageController.cs "using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

using Model;

[ApiController]
[Route(""img"")]
[EnableCors(""MainPolicy"")]
public class ImageController : ControllerBase
{
    [HttpGet(""{code}"")]
    public async Task<ActionResult> Get(
        string code,
        [FromServices]IRepository<ImageDatum> repo
    )
    {
        if (int.TryParse(code, out int id))
        {
            var query = await repo.Filter(im => im.Id == id);
            var img = query.FirstOrDefault();
            
            if (img is null)
                return NotFound();

            return File(img.Photo, ""image/jpeg"");
        }

        return BadRequest(""code needs to be a integer."");
    }

    [HttpPost]
    [DisableRequestSizeLimit]
    public async Task<ActionResult<string>> Post(
        [FromServices]IRepository<ImageDatum> repo
    )
    {
        var files = Request.Form.Files;
        if (files is null || files.Count == 0)
            return BadRequest();
        var file = Request.Form.Files[0];

        if (file.Length < 1)
            return BadRequest();

        using MemoryStream ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var data = ms.GetBuffer();

        var img = new ImageDatum();
        img.Photo = data;
        await repo.Add(img);

        var code = img.Id.ToString();
        return Ok(code);
    }
}"
ni LocationController.cs
sc LocationController.cs "using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

using DTO;
using Model;

[ApiController]
[Route(""location"")]
[EnableCors(""MainPolicy"")]
public class LocationController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<LocationDTO>>> Get(
        [FromServices] IRepository<Location> repo,
        string search = """"
    )
    {
        var query = await repo.Filter(x => x.Nome.Contains(search));
        var locations = query
            .Select(l => new LocationDTO()
            {
                ImgPath = l.Photo?.ToString() ?? """",
                Name = l.Nome
            })
            .ToList();
        return Ok(locations);
    }

    [HttpPost]
    public async Task<ActionResult> Post(
        [FromBody] LocationDTO obj,
        [FromServices] IRepository<Location> repo
    )
    {
        Location newData = new Location();
        newData.Nome = obj.Name;
        newData.Photo = string.IsNullOrEmpty(obj.ImgPath) ?
            null : int.Parse(obj.ImgPath);
        
        await repo.Add(newData);
        return Ok();
    }
}"
cd ..
ni IRepository.cs
sc IRepository.cs "using System.Linq.Expressions;

public interface IRepository<T>
{
    Task Add(T obj);
    Task<List<T>> Filter(Expression<Func<T, bool>> condition);
}"
ni ImageRepository.cs
sc ImageRepository.cs "using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

using Model;

public class ImageRepository : IRepository<ImageDatum>
{
    private FullExampleContext ctx;
    public ImageRepository(FullExampleContext ctx)
        => this.ctx = ctx;

    public async Task Add(ImageDatum obj)
    {
        await ctx.ImageData.AddAsync(obj);
        await ctx.SaveChangesAsync();
    }

    public async Task<List<ImageDatum>> Filter(Expression<Func<ImageDatum, bool>> condition)
    {
        var query = ctx.ImageData.Where(condition);
        return await query.ToListAsync();
    }
}"
ni LocationRepository.cs
sc LocationRepository.cs "using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

using Model;

public class LocationRepository : IRepository<Location>
{
    private FullExampleContext ctx;
    public LocationRepository(FullExampleContext ctx)
        => this.ctx = ctx;

    public async Task Add(Location obj)
    {
        await ctx.Locations.AddAsync(obj);
        await ctx.SaveChangesAsync();
    }

    public async Task<List<Location>> Filter(Expression<Func<Location, bool>> condition)
    {
        var query = ctx.Locations.Where(condition);
        return await query.ToListAsync();
    }
}"
sc Program.cs "using Model;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(opt =>
{
   opt.AddPolicy(""MainPolicy"", cors =>
   {
        cors
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
   });
});

builder.Services.AddScoped<FullExampleContext>();
builder.Services.AddTransient<IRepository<Location>, LocationRepository>();
builder.Services.AddTransient<IRepository<ImageDatum>, ImageRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors();

app.Run();
"
start powershell "dotnet watch"
cd ..
clear

"Informe a url do backend (ex http(S?)://localhost:(Porta) sem a barra final"
$url = Read-Host

ng new Front --routing true --style css --skip-git true
cd Front
ng add @angular/material
ng g component uploader --skip-tests
ng g component new-location-page --skip-tests
ng g component locations-page --skip-tests
ng g service location --skip-tests
ng g component location-card --skip-tests
cd src
cd app
sc app.module.ts "
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http'

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UploaderComponent } from './uploader/uploader.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule } from '@angular/material/button';
import { NewLocationPageComponent } from './new-location-page/new-location-page.component';
import { LocationsPageComponent } from './locations-page/locations-page.component';
import { LocationCardComponent } from './location-card/location-card.component';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    UploaderComponent,
    NewLocationPageComponent,
    LocationsPageComponent,
    LocationCardComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatInputModule,
    MatFormFieldModule,
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

"
ni Location.ts
sc Location.ts "export interface Location
{
    name: string;
    imgPath: string;
}"
sc location.service.ts ("import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Location } from './Location';

@Injectable({
  providedIn: 'root'
})
export class LocationService {

  constructor(private http: HttpClient) { }

  add(location: Location)
  {
    return this.http.post(""" + $url + "/location"", location)
  }
  
  all()
  {
    return this.http.get<Location[]>(""" + $url + "/location"")
  }
  
  seach(query: string)
  {
    return this.http.get<Location[]>(""" + $url + "/location?search="" + query)
  }
}
")
sc app-routing.module.ts "
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NewLocationPageComponent } from './new-location-page/new-location-page.component';
import { LocationsPageComponent } from './locations-page/locations-page.component';

const routes: Routes = [
  { path: '', component: LocationsPageComponent },
  { path: 'add', component: NewLocationPageComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
"
sc app.component.html "<router-outlet></router-outlet>"
cd uploader
sc uploader.component.html "
<input type=""file"" #file placeholder=""Choose file"" (change)=""uploadFile(file.files)"" style=""display:none;"">
<button mat-raised-button color=""primary"" (click)=""file.click()"">Carregar imagem</button>
"
sc uploader.component.ts ("import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-uploader',
  templateUrl: './uploader.component.html',
  styleUrls: ['./uploader.component.css']
})
export class UploaderComponent implements OnInit {
  progress: number = 0;
  message: string = """";
  @Output() public onUploadFinished = new EventEmitter<any>();
  
  constructor(private http: HttpClient) { }
  ngOnInit() {
  }
  uploadFile = (files: any) => {
    if (files.length === 0) {
      return;
    }
    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    
    this.http.post('" + $url + "/img', formData)
      .subscribe(result =>
      {
        this.onUploadFinished.emit(result)
      });
  }
}
")
cd ..
cd new-location-page
sc new-location-page.component.ts "import { Component } from '@angular/core';
import { FormControl } from '@angular/forms';
import { LocationService } from '../location.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-new-location-page',
  templateUrl: './new-location-page.component.html',
  styleUrls: ['./new-location-page.component.css']
})
export class NewLocationPageComponent {

  constructor(private service: LocationService, private route: Router) {}

  emailFormControl = new FormControl('', []);
  imageCode = """"
  title = """"

  onImageUpdate(code: any)
  {
    this.imageCode = code;
  }

  add()
  {
    this.service.add({
      name: this.title,
      imgPath: String(this.imageCode)
    })
    .subscribe(result =>
      {
        this.route.navigate([""/""])
      }
    )
  }
}
"
sc new-location-page.component.html "<div style=""display: flex; align-items: center; flex-direction: column; justify-content: center; gap: 20px; height: 100%;"">

    <div>
        <mat-label>Adicionar nova localização</mat-label>
    </div>

    <div>
        <mat-form-field>
            <mat-label>Titulo</mat-label>
            <input type=""text"" matInput [formControl]=""emailFormControl"" placeholder=""Lugar bonito..."" [(ngModel)]=""title"">
        </mat-form-field>
    </div>

    <div>
        <app-uploader (onUploadFinished)=""onImageUpdate(`$event)""></app-uploader>
    </div>

    <div>    
        <button mat-raised-button color=""primary"" (click)=""add()"">Salvar</button>
    </div>
    
</div>
"
cd ..
cd locations-page
sc locations-page.component.html "
<a href=""/add"">Adicionar Novo Local...</a>

<li *ngFor=""let location of locations"">
    <app-location-card [location]=""location""></app-location-card>
</li>
"
sc locations-page.component.ts ("import { Component, OnInit } from '@angular/core';
import { Location } from '../Location';
import { LocationService } from '../location.service';

@Component({
  selector: 'app-locations-page',
  templateUrl: './locations-page.component.html',
  styleUrls: ['./locations-page.component.css']
})
export class LocationsPageComponent implements OnInit {
  locations: Location[] = [];

  constructor (private service: LocationService) {}
  
  ngOnInit(): void {
    this.service.all()
      .subscribe(list =>
      {
        console.log(list)
        var newList: Location[] = []
        list.forEach(element => {
          newList.push({
            name: element.name,
            imgPath: """ + $url + "/img/"" + element.imgPath
          })
        });
        this.locations = newList
      })
  }
}")
cd ..
cd location-card
sc location-card.component.ts "
import { Component, Input } from '@angular/core';
import { Location } from '../Location';

@Component({
  selector: 'app-location-card',
  templateUrl: './location-card.component.html',
  styleUrls: ['./location-card.component.css']
})
export class LocationCardComponent {
  @Input() location: Location = { name: """", imgPath: """"};
}
"
sc location-card.component.html "
<div style=""display: flex; align-items: center; flex-direction: column;"">
    <div>
        {{location.name}}
    </div>
    <div>
        <img src={{location.imgPath}} width=""300px""/>
    </div>
</div>
"
cd ..
cd ..
cd ..
start powershell "npm start"
start chrome http://localhost:4200/