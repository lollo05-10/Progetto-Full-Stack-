export interface AppReport {
  id: number;
  title: string;
  description: string;
  userId: number;
  reportDate: string;
  longitude: number;
  latitude: number;
  categoryNames: string[];
  images: ImageDTO[];
}
export interface AppReportPost {
  title: string;
  description: string;
  userId: number;
  reportDate: string;
  longitude: number;
  latitude: number;
  categoryNames: string[];
  images: ImageDTOPost[];
}
export interface ImageDTO {
  base64?: string; // Deprecato, usare path
  path?: string; // Percorso relativo dell'immagine (es: /images/xxx.jpg)
}
export interface ImageDTOPost {
  path: string;
}
