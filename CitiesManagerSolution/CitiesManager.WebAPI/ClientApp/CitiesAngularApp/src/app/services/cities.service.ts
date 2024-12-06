import { Injectable } from '@angular/core';
import { City } from '../models/city';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from "rxjs";

const API_BASE_URL: string = "https://localhost:7158/api/";

@Injectable({
  providedIn: 'root'
})
export class CitiesService {
  cities: City[] = [];
  constructor(private httpClient: HttpClient) {
    //this.cities = [
    //  //private httpClient:HttpClient
    //  new City("101", "New York"),
    //  new City("101", "New Delhi"),
    //  new City("101", "Patna"),
    //  new City("101", "Sydney")
    //];
  }

  public getCities(): Observable<City[]> {
    //return this.cities;
    let headers = new HttpHeaders();
    headers = headers.append("Authorization", `Bearer ${localStorage['token']}`);
    return this.httpClient.get<City[]>(`${API_BASE_URL}v1/Cities`, { headers: headers })
    //return this.httpClient.get<City[]>("https://localhost:7158/api/v1/Cities", {headers:headers});
    //Observable<City[]>
  }

  public postCity(city: City): Observable<City> {
    let headers = new HttpHeaders();
    headers = headers.append("Authorization", `Bearer ${localStorage['token']}`);
    return this.httpClient.post<City>(`${API_BASE_URL}v1/Cities`, city, { headers: headers })
    //return this.httpClient.get<City>("https://localhost:7158/api/v1/Cities", { headers: headers });
    //Observable<City[]>
  }

  public putCity(city: City): Observable<string> {
    let headers = new HttpHeaders();
    headers = headers.append("Authorization", `Bearer ${localStorage['token']}`);

    return this.httpClient.put<string>(`${API_BASE_URL}v1/Cities/${city.cityID}`, city, { headers: headers })
  }

  public deleteCity(cityID: string | null): Observable<string> {
    let headers = new HttpHeaders();
    headers = headers.append("Authorization", `Bearer ${localStorage['token']}`);

    return this.httpClient.delete<string>(`${API_BASE_URL}v1/cities/${cityID}`, { headers: headers })
  }

}
