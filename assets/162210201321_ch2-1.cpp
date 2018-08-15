# include <iostream>
using namespace std;

const int Maxsize=10;
template <class T>      //使用类模板 
class SeqList
{
	public:
	   SeqList(T a[],int n);
	   void reverse (T a[],int n);
	   void print(T a[],int n);
	 
	 private: 
	 T data[Maxsize];   
	
};


template <class T>
SeqList<T> :: SeqList(T a[],int n)    //声明构造函数 
{
	for(int i=0; i<n; i++)
	
		data[i]=a[i];
			
}

template <class T>
void SeqList<T> :: reverse(T a[],int n)    //声明逆置函数 
{
		for(int j=0; j<n/2; j++){   // j < n/2,如果 j<n,则进行两次翻转，则会保持原数组 
				T temp;
				temp=a[j];
				a[j]=a[n-j-1];
				a[n-j-1]=temp;
            }
          
}
		
template <class T>
void SeqList<T> :: print(T a[],int n)
{
	for(int i=0; i<n; i++)  
	cout<<" "<<a[i];
				
}		
		
			
int main(){
		
			int arr[6]={1,2,7,8,9,10};
			
		    SeqList <int> list(arr,6);  //生成SeqList的对象 
		    
		    cout<<"初始数组为："<<endl;
		    list.print(arr,6);
		    cout<<endl;
			
			cout<<"转置数组为："<<endl;
			list.reverse(arr,6); 
		    list.print(arr,6);
		    cout<<endl;
		    
		    return 0;
				
		}
		
		
			
			
			
		



  
    	
    	
	 
	
 


