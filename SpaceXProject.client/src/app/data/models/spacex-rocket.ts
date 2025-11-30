export interface SpaceXRocket {
  id: string;
  name: string;
  type: string;
  description: string;
  height: { meters: number; feet: number };
  diameter: { meters: number; feet: number };
  mass: { kg: number; lb: number };
  flickr_images: string[];
}
