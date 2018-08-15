#include <iostream>
using namespace std;

template <class T>  //去除单调不递减单链表中的值相同的节点 
struct Node
{
	T data;
	Node <T> * next; 
};


template <class T>
class LinkList
{
	public:
		LinkList(T a[],int n);
		void ShowList();
		void DeleteSam();
	    
	private:
		Node<T> * first; 
 };
 
template <class T>  		//头插法建立单链表 
 LinkList<T> :: LinkList(T a[], int n)  	//带有参数的构造函数 
 {
 	  
	 first = new Node <T>;
	 first -> next =NULL;
 	 for(int i=0;i<n;i++)
 	 {
 		Node<T> * s;
 		s = new Node <T>;
 		s->data = a[i];
 		s->next = first->next;
 		first->next = s;
 			
	 }
	 
 	}
 	
 template <class T>  
 void  LinkList<T> :: ShowList()   //输出函数 
 {  
    Node<T> * s;
    s=first->next;
 	while(s!=NULL){ 
 	
 	cout<< s->data <<" ";
 	s=s->next;
	 	
 }
 	
 }
 
 template <class T>  
 void  LinkList<T> :: DeleteSam()
 {  
    Node<T> * p;
 	p=first;
	while(p->next!=NULL)
			if(p->data==p->next->data){
				Node<T> * q; 
				q=p->next;
				p->next=q->next;   //将值相同节点中的后一个节点删除 
				delete q;
				
			} 
			else p=p->next;
 	
 }
 
 int main(){
 	
     int a[]={1,2,3,5,5};
 	 LinkList <int > list(a,5);
 	 
 	 
 	 
 	cout<<"删除重复元素之前，单链表中的数据依次为："<<endl; 
 	list.ShowList();
 	cout<<endl;
 	
 	cout<<"删除重复元素之后，单链表中的数据依次为："<<endl; 
 	list.DeleteSam();
 	list.ShowList();
 	cout<<endl;
 	
 	return 0;
 }
