export interface AppReport {
  id: number;
  description: string;
  title: string;
  date: string;
  categories: string[];
  images: string[];
  latitude: number;
  longitude: number;
  distance?: number;
}