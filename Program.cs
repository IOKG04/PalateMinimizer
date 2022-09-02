using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace PalateMinimizer;

class Program{
	static Image<Rgba32> inImg;
	static Image<Rgba32> outImg;

	static Rgba32[] palate;

	static void Main(string[] args){
		if(args.Length != 0 && (args[0] == "--help" || args[0] == "-h")){
			Console.WriteLine("Usage:\n PalateMinimizer input-path output-path color-0 color-1 ...");
			return;
		}

		if(args.Length < 4){
			Console.WriteLine("Error 0: Not enough arguments");

			Console.WriteLine("Usage:\n PalateMinimizer input-path output-path color-0 color-1 ...");
			return;
		}

		if(!File.Exists(args[0])){
			Console.WriteLine("Error 1: Input image does not exist.");
			return;
		}

		if(File.Exists(args[1])){
			Console.WriteLine("Output file already exists. Overwrite? (Y | n)");
			string str = Console.ReadLine().ToLower();
			if(str == "n" || str == "no"){
				Console.WriteLine("Error 2: Output file exists.");
				return;
			}
		}

		if(!AllValiteHexcodes(ref args, 2)){
			Console.WriteLine("Error 3: Given colors are not in hexcode format");
			return;
		}

		inImg = (Image<Rgba32>)Image.Load(args[0]);
		outImg = new Image<Rgba32>(inImg.Width, inImg.Height);

		palate = new Rgba32[args.Length - 2];
		for(int i = 2; i < args.Length; i++){
			palate[i - 2] = Rgba32.ParseHex(args[i]);
		}

		Random r = new Random();
		for(int x = 0; x < inImg.Width; x++){
			for(int y = 0; y < inImg.Height; y++){
				int closest1 = 0;
				int closest2 = 0;
				int closestDistance1 = 1000;
				int closestDistance2 = 1000;
				for(int i = 0; i < palate.Length; i++){
					int distance = Math.Abs(inImg[x, y].R - palate[i].R) + Math.Abs(inImg[x, y].G - palate[i].G) + Math.Abs(inImg[x, y].B - palate[i].B);
					if(distance < closestDistance1){
						closestDistance2 = closestDistance1;
						closestDistance1 = distance;
						closest2 = closest1;
						closest1 = i;
					}
					else if(distance < closestDistance2){
						closestDistance2 = distance;
						closest2 = i;
					}
				}
				if(closestDistance1 == closestDistance2){
					outImg[x, y] = palate[r.Next(0, 2) == 0 ? closest1 : closest2];
				}
				else{
					int ri = r.Next(closestDistance1 + closestDistance2);
					if(ri < closestDistance1) outImg[x, y] = palate[closest2];
					else outImg[x, y]  = palate[closest1];
				}
			}
		}

		outImg.Save(args[1]);
	}

	static bool AllValiteHexcodes(ref string[] arr, int start){
		for(int i = start; i < arr.Length; i++){
			if(!arr[i].StartsWith("#")){
				arr[i] = arr[i].Insert(0, "#");
			}

			try{
				int.Parse(arr[i].Remove(0, 1), System.Globalization.NumberStyles.HexNumber);
			}
			catch{
				return false;
			}
		}
		return true;
	}
}
