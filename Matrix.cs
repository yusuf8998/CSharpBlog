using System;
using System.Collections.Generic;

namespace matrix
{
    public class Matrix<T>
    {
        public int SizeX;public int SizeY;public int Size;
        private T[,] array;
		
        public Matrix(int x, int y){
		array = new T[x,y];
		SizeX = x; SizeY = y; Size = x*y;
        }
		
	public void clear(){
		array = new T[SizeX,SizeY];
	}
		
        public void setElement(int x, int y, T e){
		if(x < SizeX && y < SizeY){
			array[x,y] = e;
		}
        }
		
	public void setElement((int X, int Y) pos, T e){
		if(pos.X < SizeX && pos.Y < SizeY){
			array[pos.X,pos.Y] = e;
		}
        }
		
	public void addToEnd(T e){
		for(int y=0; y<SizeY; y++){
			for(int x=0; x<SizeX; x++){
				if((object)getElement(x, y) == (object)default(T)){
					setElement(x, y, e);
					return;
				}
			}
		}
	}
		
        public T getElement(int x, int y){
		if(x < SizeX && y < SizeY){
			return array[x,y];
		}
		return default(T);
        }
		
	public T getElement((int X, int Y) t){
		if(t.Item1 < SizeX && t.Item2 < SizeY){
			return array[t.X,t.Y];
		}
		return default(T);
        }
		
	public (int X, int Y) getPosByNumber(int number){
		int x = (number - 1) % SizeX;
		int y = (number - 1) / SizeX;
		return (X:x, Y:y);
	}
		
        public int getElementNumber(int x, int y){
		if(x < SizeX && y < SizeY){
			return y * (SizeX - 1) + x + 1;
		}
		return -1;
        }
		
	public void removeElement(int x, int y){
		if(x < SizeX && y < SizeY){
			array[x,y] = default(T);
		}
	}
		
	public void removeElement((int X, int Y) pos){
		if(pos.X < SizeX && pos.Y < SizeY){
			array[pos.X,pos.Y] = default(T);
		}
	}
		
	public void removeAt(int num){
		removeElement(getPosByNumber(num));
	}
		
	public void removeAll(Predicate<T> predicate){
		List<(int X, int Y)> all = findAll(predicate);
		Action<(int X, int Y)> act = ((int X, int Y) pos) => removeElement(pos); 
		all.ForEach(act);
	}
		
	public bool checkElement(int x, int y){
		return (object)array[x,y] != (object)default(T);
	}

	public bool checkElement(int x, int y, T e){
		return array[x,y].Equals(e);
	}
		
	public (int X, int Y) find(Predicate<T> predicate){
		for(int x = 0; x < SizeX; x++){
			for(int y = 0; y < SizeY; y++){
			    if(predicate.Invoke(array[x,y])) return (X:x, Y:y);
			}
		}
		return (X:-1, Y:-1);
	}
		
	public (int X, int Y) findLast(Predicate<T> predicate){
		for(int x = SizeX - 1; x >= 0; x--){
			for(int y = SizeY - 1; y >= 0; y--){
				if(predicate.Invoke(array[x,y])) return (X:x, Y:y);
			}
		}
		return (X:-1, Y:-1);
	}
		
	public List<(int X, int Y)> findAll(Predicate<T> predicate){
		List<(int X, int Y)> result = new List<(int X, int Y)>();
		for(int x = SizeX - 1; x >= 0; x--){
			for(int y = SizeY - 1; y >= 0; y--){
				if(predicate.Invoke(array[x,y])){ result.Add((X:x, Y:y)); }
			}
		}
		return result;
	}
		
	public bool exists(Predicate<T> predicate){
		for(int x = 0; x < SizeX; x++){
			for(int y = 0; y < SizeY; y++){
			    if(predicate.Invoke(array[x,y])) return true;
			}
		}
		return false;
	}
		
	public bool trueForAll(Predicate<T> predicate){
		List<(int X, int Y)> l = findAll(predicate);
		return l.Count == Size ? true : false;
	}
		
	public void forEach(Action<T> act){
		for(int x=0; x<SizeX; x++){
			for(int y=0; y<SizeY; y++){
				act(getElement(x, y));
			}
		}
	}
		
	public void setArray(T[,] arr){
		array = arr;
		syncArrayAndAttributes();
	}

	private void syncArrayAndAttributes(){
		SizeX = array.GetLength(0); SizeY = array.GetLength(1); Size = SizeX * SizeY;
	}
    }
	
    class Debug{
        static void Main(string[] args){
            Matrix<string> m = new Matrix<string>(1, 1);
            while(true){
                string command = Console.ReadLine();

                if(command == "break"){
                    break;
                }
                else if(command == "init"){
                    int x = Convert.ToInt32(Console.ReadLine());
                    int y = Convert.ToInt32(Console.ReadLine());
                    m = new Matrix<string>(x, y);
                }
                else if(command == "read"){
                    int x = Convert.ToInt32(Console.ReadLine());
                    int y = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine(m.getElement(x, y));
                }
                else if(command == "write"){
                    int x = Convert.ToInt32(Console.ReadLine());
                    int y = Convert.ToInt32(Console.ReadLine());
                    m.setElement(x, y, Console.ReadLine());
                }
                else if(command == "check"){
                    int x = Convert.ToInt32(Console.ReadLine());
                    int y = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine(m.checkElement(x, y));
                }
                else if(command == "compare"){
                    int x = Convert.ToInt32(Console.ReadLine());
                    int y = Convert.ToInt32(Console.ReadLine());
                    string e = Console.ReadLine();
                    Console.WriteLine(m.checkElement(x, y, e));
                }
                else if(command == "find"){
                    string t = Console.ReadLine();
                    Predicate<string> predicate = new Predicate<string>(a => a == t);
                    (int X, int Y) o = m.find(predicate);
                    Console.WriteLine(o.X + "x" + o.Y);
                }
		else if(command == "foreach"){
			Action<string> act = (string s) => Console.WriteLine(s != null ? s : "-null-");
			m.forEach(act);
		}
		else if(command == "add"){
			string t = Console.ReadLine();
			m.addToEnd(t);
		}
		else if(command == "display"){
			for(int y=0; y<m.SizeY; y++){
				int x=0;
				while(x < m.SizeX){
					Console.Write("- " + m.getElement(x, y) + " -");
					if(x == m.SizeX - 1){
						Console.WriteLine("");
					}
					x++;
				}
			}
		}
		else if(command == "clear"){
			m.clear();
		}
                else{
                    Console.WriteLine("Command unknown");
                }
            }
        }
    }
}
